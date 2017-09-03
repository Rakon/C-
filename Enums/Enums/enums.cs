using System;

namespace Enums
{
    public enum ShippingMethod
    {
        regularAirmail = 1,
        registeredAirlmail = 2,
        express = 3
    }
    class enums
    {
        static void Main(string[] args)
        {
            var method = ShippingMethod.express;
            Console.WriteLine((int)method);

            var methodID = 3;
            Console.WriteLine((ShippingMethod)methodID);

            Console.WriteLine(method.ToString());

            var methodName = "Express";
            var shippingMethods = (ShippingMethod) Enum.Parse(typeof (ShippingMethod), methodName);
        }
    }
}
