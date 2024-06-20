namespace WebAppPeopleCRUD
{
    public static class DotEnv
    {
        public static void Load(WebApplicationBuilder buider)
        {
            var filepath = buider.Environment.ContentRootPath + "\\.env";
            if (!File.Exists(filepath))
            {
                return;
            }
            foreach (var file in File.ReadAllLines(filepath))
            {
                if (!file.Contains('='))
                {
                    continue;
                }

                var parts = file.Split('=');

                Environment.SetEnvironmentVariable(parts[0], string.Join("=", parts[1..]));
            }
        }
    }
}
