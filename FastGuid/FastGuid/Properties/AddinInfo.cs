using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "FastGuid",
    Namespace = "FastGuid",
    Version = "1.0"
)]

[assembly: AddinName("FastGuid")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Quickly create new GUIDs through the Search Bar and copy them to your clipboard.")]
[assembly: AddinAuthor("Matthew Robbins")]
