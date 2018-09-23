using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineDemo
{

    public class Person {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get { return Convert.ToInt16((DateOfBirth - DateTime.Now).TotalDays / 365); } }
        public bool IsCitiZen { get; set; }
        public bool IsImmigrantStatusValid { get; set; }
        public bool HasCriminalRecord { get; set; }
        public int CreditScore { get; set; }
    }

    public interface IDecisionNode<Client>
    {
        Func<Client, bool> Condition { get; set; }
        string ConditionDesc { get; set; }
        IDecisionNode<Client> TrueNode { get; set; }
        IDecisionNode<Client> FalseNode { get; set; }

        bool Evaluate(Client client);
    }
    public class DecisionNode<Client> : IDecisionNode<Client>
    {
        public Func<Client, bool> Condition { get; set; }
        public string ConditionDesc { get; set; }
        public IDecisionNode<Client> TrueNode { get; set; }
        public IDecisionNode<Client> FalseNode { get; set; }

        public bool Evaluate(Client client)
        {
            var conditionResult = Condition(client);
            Console.WriteLine("Condition is={0} Result={1}", ConditionDesc, conditionResult);
            return conditionResult ? (TrueNode == null ? conditionResult: TrueNode.Evaluate(client)) 
                                    : FalseNode == null ? conditionResult : FalseNode.Evaluate(client); 
        }
    }

    public interface IRule
    {
        bool Evaluate(Person person);
    }
    public class Rules : IRule
    {
        IDecisionNode<Person> _startLoanNode;
        
        public Rules()
        {
            _startLoanNode = new DecisionNode<Person>
            {
                Condition = (x) => x.IsCitiZen,
                ConditionDesc = "(x) => x.IsCitiZen"
            };
            var conditionIfCitizenOrValidImmigrant= new DecisionNode<Person>()
            {
                Condition = x => !x.HasCriminalRecord,
                ConditionDesc = "x => x.HasCriminalRecord",
                TrueNode = new DecisionNode<Person>() { Condition = x => x.CreditScore > 500, ConditionDesc = "x => x.CreditScore > 500" }
            };
            _startLoanNode.FalseNode = new DecisionNode<Person>() { Condition = x => x.IsImmigrantStatusValid, ConditionDesc = "x.IsImmigrantStatusValid", TrueNode = conditionIfCitizenOrValidImmigrant };
            _startLoanNode.TrueNode = conditionIfCitizenOrValidImmigrant;
        }

        public bool Evaluate(Person person)
        {
            return _startLoanNode.Evaluate(person);
        }

    }

    
}
