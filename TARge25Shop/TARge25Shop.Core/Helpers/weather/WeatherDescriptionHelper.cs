namespace TARge25Shop.Core.Helpers.weather;

public class WeatherDescriptionHelper
{
    public static string GetEstonianDescription(int weatherId, string fallbackDescription)
    {
        return weatherId switch
        {
            >= 200 and < 300 => "Äike",
            >= 300 and < 400 => "Uduvihm",
            500 => "Kerge vihm",
            501 => "Mõõdukas vihm",
            502 or 503 or 504 => "Tugev vihm",
            511 => "Jäide",
            >= 520 and < 600 => "Hoovihm",
            600 => "Kerge lumesadu",
            601 => "Lumesadu",
            602 => "Tugev lumesadu",
            >= 611 and < 630 => "Lörts / lumesadu",
            >= 700 and < 800 => "Udu / vine",
            800 => "Selge taevas",
            801 => "Vähesed pilved",
            802 => "Vahelduvad pilved",
            803 or 804 => "Pilvine",
            _ => !string.IsNullOrEmpty(fallbackDescription) ? fallbackDescription : "Teadmata"
        };
    }
}