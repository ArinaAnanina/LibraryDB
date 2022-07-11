using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDB.Controllers;
using LibraryDB.DB;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LibraryDB.Exceptions;
using System.Text.RegularExpressions;

namespace LibraryDB.Services
{
    public class PersonService
    {
        ApplicationContext _context;
        private readonly ILogger<PersonController> _logger;

        DbSet<Person> personSet
        {
            get
            {
                return _context?.Person;
            }
        }
        public PersonService(ILogger<PersonController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addPerson(Person newPerson)
        {
            EntityEntry<Person> entry = await personSet.AddAsync(newPerson);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Person>> getPerson()
        {
            return personSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Person> getPerson(int id)
        {
            Person result = await personSet
                .Where(o => o.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Person", id);
            }

            return result;
        }
        public async Task updatePerson(Person person)
        {
            if (person.Name == null || person.Surname == null || person.MiddleName == null || person.Domicile == null || person.PhoneNumber == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Person per = personSet.Find(person.Id);
            per.Name = person.Name;
            per.Surname = person.Surname;
            per.MiddleName = person.MiddleName;
            per.BirthDate = person.BirthDate;
            per.Domicile = person.Domicile;
            per.PhoneNumber = person.PhoneNumber;

            await _context.SaveChangesAsync();
        }
        public async Task removePerson(int? id)
        {
            Person person = personSet.Where(o => o.Id == id).FirstOrDefault();
            if (person == null)
            {
                throw new NotFoundException("Person", id);
            }
            EntityEntry<Person> entry = personSet.Remove(person);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
