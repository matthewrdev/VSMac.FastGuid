using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Components.MainToolbar;
using MonoDevelop.Core.Text;
using MonoDevelop.Ide.CodeCompletion;
using Xwt;

namespace FastGuid
{
    class FastGuidSearchCategory : SearchCategory
    {
        public FastGuidSearchCategory()
            : base("Create Guid")
        {
        }

        class CreateGuidResult : SearchResult
        {
            readonly string input;
            public CreateGuidResult(string pattern,
                                       int rank)
                : base(pattern, "Create Guid", rank)
            {
                input = pattern;
            }

            public override void Activate()
            {
                var results = Create(input);

                var message = $"Copied '{results.FirstOrDefault()}' to the clipboard";
                if (results.Count > 1)
                {
                    message = $"Copied {results.Count} GUIDs to the clipboard";
                    Clipboard.SetText(string.Join(Environment.NewLine, results));
                }
                else
                {
                    Clipboard.SetText(results.FirstOrDefault().ToString());
                }

                Xwt.Application.Invoke(() =>
                {
                    MonoDevelop.Ide.IdeApp.Workbench.StatusBar.ShowMessage(message);
                });
            }

            IReadOnlyList<Guid> Create(string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return new List<Guid>();
                }

                var components = input.Trim().Split(':');
                if (components.Length <= 1)
                {
                    return new List<Guid>()
                    {
                        Guid.NewGuid()
                    };
                }

                var totalGuids = components[1];
                if (!int.TryParse(totalGuids, out var count))
                {
                    return new List<Guid>()
                    {
                        Guid.NewGuid()
                    };
                }

                List<Guid> result = new List<Guid>();
                for (int i = 0; i < count; ++i)
                {
                    result.Add(Guid.NewGuid());
                }

                return result;
            }

            public override string GetMarkupText(bool selected)
            {
                return "Create Guid";
            }

            public override string GetDescriptionMarkupText()
            {
                var result = Create(input);
                if (result.Count > 1)
                {
                    return $"Create {result.Count} new GUIDS and copy them to the clipboard on new lines.";
                }
                else
                {
                    return $"Create a new GUID and copy it to the clipboard.";
                }
            }

            public override bool CanActivate => true;
            
            public override Task<TooltipInformation> GetTooltipInformation(CancellationToken token)
            {
                var tooltipInfo = new TooltipInformation()
                {
                    SignatureMarkup = this.Description,
                };


                tooltipInfo.AddCategory("Description", GetDescriptionMarkupText());

                return Task.FromResult(tooltipInfo);
            }

            public string GetFileName(int item)
            {
                return string.Empty;
            }
        }

        public override Task GetResults(ISearchResultCallback searchResultCallback, SearchPopupSearchPattern pattern, System.Threading.CancellationToken token)
        {
            return Task.Run(delegate
            {
                var matcher = StringMatcher.GetMatcher(pattern.Pattern, false);

                if (matcher.CalcMatchRank("Create Guid", out var rank))
                {
                    var input = pattern.UnparsedPattern;
                    searchResultCallback.ReportResult(new CreateGuidResult(input, rank));
                }
            }, token);
        }

        readonly string[] tags = { "Create Guid" };
        public override string[] Tags => tags;

        public override bool IsValidTag(string tag)
        {
            var matcher = StringMatcher.GetMatcher(tag, false);
            return matcher.CalcMatchRank("Create Guid", out var rank);
        }
    }
}

