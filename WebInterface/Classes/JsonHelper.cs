﻿namespace WebInterface.Classes
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Models;

    public static class JsonHelper
    {
        public static List<GithubRepository> GithubRepositoriesFromJson(string json) => JsonConvert.DeserializeObject<List<GithubRepository>>(json, Converter.Settings);

        public static string GithubRepositoriesToJson(List<GithubRepository> self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static List<GithubRepositoryVersion> GithubRepositoryVersionFromJson(string json) => JsonConvert.DeserializeObject<List<GithubRepositoryVersion>>(json, Converter.Settings);

        public static string GithubRepositoryVersionToJson(List<GithubRepositoryVersion> self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static GeoNodeDocument GeoNodeDocumentFromJson(string json) => JsonConvert.DeserializeObject<GeoNodeDocument>(json, Converter.Settings);

        public static string GeoNodeDocumentToJson(GeoNodeDocument self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static GithubContent[] GithubContentFromJson(string json) => JsonConvert.DeserializeObject<GithubContent[]>(json, Converter.Settings);

        public static string GithubContentToJson(this GithubContent[] self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static DockerhubRepositoryTags DockerhubRepositoryTagsFromJson(string json) => JsonConvert.DeserializeObject<DockerhubRepositoryTags>(json, Converter.Settings);

        public static string DockerhubRepositoryTagsToJson(this DockerhubRepositoryTags self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static DockerhubRepository DockerhubRepositoryFromJson(string json) => JsonConvert.DeserializeObject<DockerhubRepository>(json, Converter.Settings);

        public static string DockerhubRepositoryToJson(this DockerhubRepository self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static GeoNodeDocumentData GeoNodeDocumentDataFromJson(string json) => JsonConvert.DeserializeObject<GeoNodeDocumentData>(json, Converter.Settings);

        public static string GeoNodeDocumentDataToJson(this GeoNodeDocumentData self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static List<GithubBranch> GithubBranchFromJson(string json) => JsonConvert.DeserializeObject<List<GithubBranch>>(json, Converter.Settings);

        public static string GithubBranchToJson(this List<GithubBranch> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
