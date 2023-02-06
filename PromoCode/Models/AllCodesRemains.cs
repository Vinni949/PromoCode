namespace PromoCode.Models
{
    public class AllCodesRemains
    {
        /// <summary>
        /// Все промокоды
        /// </summary>
        public int all { get; set; }

        /// <summary>
        /// активированные
        /// </summary>
        public int? activated { get;set; }

        /// <summary>
        /// оплаченные неактивированные
        /// </summary>
        public int? paidAndInactive { get; set; }

        /// <summary>
        /// выданные неоплаченные
        /// </summary>
        public int? issuedAndUnpaid { get; set; }

        /// <summary>
        /// невыданны
        /// </summary>
        public int? unreleased { get; set; }
    }
}
