namespace DataProvider.Providers.Cards.Amex.Dto
{
    public class AmexLoginResponse
    {
        public object BankName { get; set; }
        public string FirstName { get; set; }
        public bool IsBank { get; set; }
        public object IsCaptcha { get; set; }
        public bool IsSeenPop { get; set; }
        public string LastLoginDate { get; set; }
        public string LastName { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
