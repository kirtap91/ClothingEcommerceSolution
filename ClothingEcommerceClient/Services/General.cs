namespace ClothingEcommerceClient.Services
{
    public static class General
    {
        public static string GetDescription(string description)
        {
            string appendDots = "...";
            int maxLength = 100;
            int descriptionLength = description.Length;
            return descriptionLength > maxLength ? $"{description.Substring(0, 100)}{appendDots}" : description;
        }
    }
}
