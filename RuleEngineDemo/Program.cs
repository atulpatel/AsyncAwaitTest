using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Person> persons = new List<Person>() { new Person() { Name = "Atul", CreditScore = 550, DateOfBirth = new DateTime(1980, 7, 7), HasCriminalRecord = false, IsCitiZen = false, IsImmigrantStatusValid = true }
            ,new Person() { Name = "Mital", CreditScore = 550, DateOfBirth = new DateTime(1980, 7, 7), HasCriminalRecord = false, IsCitiZen = false, IsImmigrantStatusValid = false }
            ,new Person() { Name = "Sheryl", CreditScore = 550, DateOfBirth = new DateTime(2011, 3, 4), HasCriminalRecord = false, IsCitiZen = false, IsImmigrantStatusValid = true}
            ,new Person() { Name = "Monil", CreditScore = 550, DateOfBirth = new DateTime(2011, 3, 4), HasCriminalRecord = false, IsCitiZen = true, IsImmigrantStatusValid = true}
            };
            IRule rules = new Rules();

            foreach (var person in persons)
            {
                var iseligibleforloan = rules.Evaluate(person);

                if (iseligibleforloan)
                    Console.WriteLine("{0} Is Eligible", person.Name);
                else
                    Console.WriteLine("{0} Is Not Eligible", person.Name);

            }

            Console.ReadKey();
        }
    }
}
