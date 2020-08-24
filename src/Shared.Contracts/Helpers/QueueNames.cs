using System;

namespace Shared.Contracts.Helpers
{
    public class QueueNames
    {
        private const string RabbitUri = "queue:";
        
        public static string GetQueueName(string key)
        {
            return key.PascalToKebabCaseMessage();
        }
            
        public static Uri GetMessageUri(string key)
        {
            return new Uri(RabbitUri + GetQueueName(key));
        }
        public static Uri GetActivityUri(string key)
        {
            var kebabCase =  key.PascalToKebabCaseActivity();
            if (kebabCase.EndsWith('-'))
            {
                kebabCase = kebabCase.Remove(kebabCase.Length - 1);
            }
            return new Uri(RabbitUri + kebabCase + '_'  + "execute");
        }
    }
}