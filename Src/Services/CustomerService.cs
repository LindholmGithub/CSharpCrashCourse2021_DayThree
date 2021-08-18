using System;
using System.Collections.Generic;
using System.IO;
using CrashCourse2021ExercisesDayThree.DB.Impl;
using CrashCourse2021ExercisesDayThree.Models;

namespace CrashCourse2021ExercisesDayThree.Services
{
    public class CustomerService
    {
        CustomerTable db; 
        public CustomerService()
        {
            this.db = new CustomerTable();
        }

        //Create and return a Customer Object with all incoming properties (no ID)
        internal Customer Create(string firstName, string lastName, DateTime birthDate)
        {
            Customer newCustomer = new Customer();

            if (firstName.Length < 2)
            {
                throw new ArgumentException(Constants.FirstNameException);
            }
            
            if (firstName != null && lastName != null && birthDate != null)
            {
                newCustomer.FirstName = firstName;
                newCustomer.LastName = lastName;
                newCustomer.BirthDate = birthDate;
            }
            return newCustomer;
        }

        //db has an Add function to add a new customer!! :D
        //We can reuse the Create function above..
        internal Customer CreateAndAdd(string firstName, string lastName, DateTime birthDate)
        {
            var newCustomer = Create(firstName, lastName, birthDate);
            db.AddCustomer(newCustomer);
            return newCustomer;
        }

        //Simple enough, Get the customers from the "Database" (db)
        internal List<Customer> GetCustomers()
        {
            return db.GetCustomers();
        }

        //Maybe Check out how to find in a LIST in c# Maybe there is a Find function?
        public Customer FindCustomer(int customerId)
        {
            if (customerId <= 0)
            {
                throw new InvalidDataException(Constants.CustomerIdMustBeAboveZero);
            }
            return db.GetCustomers().Find(c => c.Id == customerId);
        }
        
        /*So many things can go wrong here...
          You need lots of exceptions handling in case of failure and
          a switch statement that decides what property of customer to use
          depending on the searchField. (ex. case searchfield = "id" we should look in customer.Id 
          Maybe add searchField.ToLower() to avoid upper/lowercase letters)
          Another thing is you should use FindAll here to get all that matches searchfield/searchvalue
          You could also make another search Method that only return One Customer
           and uses Find to get that customer and maybe even test it.
        */
        public List<Customer> SearchCustomer(string searchField, string searchValue)
        {
            int id;
            var foundCustomer = new List<Customer>();
            if (searchField == null)
            {
                throw new InvalidDataException(Constants.CustomerSearchFieldCannotBeNull);
            }
            if (searchValue == null)
            {
                throw new InvalidDataException(Constants.CustomerSearchValueCannotBeNull);
            }
            switch (searchField.ToLower())
            {
                case "id":
                    if (Int32.TryParse(searchValue, out id))
                    {
                        if (int.Parse(searchValue) >= 1)
                        {
                            foreach (var customer in GetCustomers())
                            {
                                if (customer.Id.ToString().Equals(searchValue))
                                {
                                    foundCustomer.Add(customer);
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidDataException(Constants.CustomerIdMustBeAboveZero);
                        }
                    }
                    else if (!int.TryParse(searchValue,out id))
                    {
                        throw new InvalidDataException(Constants.CustomerSearchValueWithFieldTypeIdMustBeANumber);
                    }
                    
                    return foundCustomer;
                
                case "firstname":
                    foreach (var customer in GetCustomers())
                    {
                        if (customer.FirstName.StartsWith(searchValue))
                        {
                            foundCustomer.Add(customer);
                        }
                    }
                    return foundCustomer;
                
                case "lastname":
                    foreach (var customer in GetCustomers())
                    {
                        if (customer.LastName.StartsWith(searchValue))
                        {
                            foundCustomer.Add(customer);
                        }
                    }
                    return foundCustomer;
                default:
                    throw new InvalidDataException(Constants.CustomerSearchFieldNotFound);
            }
        }
    }
}
