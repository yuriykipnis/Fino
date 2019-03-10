using System;


namespace DataProvider.Providers.Cards.Cal.Dto
{
    public class CalLoginResponse
    {
        public String Hash{ get; set; }
        public int InnerLoginType { get; set; }
        public String Token { get; set; }
    }
}
