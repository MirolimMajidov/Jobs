
using System.ComponentModel.DataAnnotations;

namespace Jobs.SharedModel.Helpers
{
    public enum JobType
    {
        [Display(Name = "Hourly")]
        Hourly,

        [Display(Name = "Fixed-price")]
        FixedPrice
    }

    public enum JobDuration
    {
        [Display(Name = "Less than 1 week")]
        LessThanWeek,

        [Display(Name = "Less than 1 month")]
        LessThanMonth,

        [Display(Name = "From 1 to 3 months")]
        FromOneToThreeMonths,

        [Display(Name = "Less than 6 months")]
        LessThanSixMonths,

        [Display(Name = "More than 6 months")]
        MoreThanSixMonths
    }

    public enum GenderStatus
    {
        [Display(Name = "Not selected")]
        NotSelected,

        [Display(Name = "Female")]
        Female,

        [Display(Name = "Male")]
        Male
    }

    public enum CurrencyType
    {
        [Display(Name = "USD")]
        USD = 0,

        [Display(Name = "RUB")]
        RUB = 1,

        [Display(Name = "UZB")]
        UZB = 2,

        [Display(Name = "TJS")]
        TJK = 3,
    }

    public enum PaymentStatus
    {
        Process = 0,
        Success = 1,
        Rejected = 2,
        Cancel = 3
    }
}
