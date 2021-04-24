
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
}
