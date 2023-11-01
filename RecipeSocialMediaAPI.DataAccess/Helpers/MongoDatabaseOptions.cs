﻿using System.ComponentModel.DataAnnotations;

namespace RecipeSocialMediaAPI.DataAccess.Helpers;

public class MongoDatabaseOptions
{
    public const string CONFIGURATION_SECTION = "MongoDB";

    public string ConnectionString { get; set; } = string.Empty;
    public string ClusterName { get; set; } = string.Empty;
}
