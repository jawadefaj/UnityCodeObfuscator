using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin (
	"obfuscator",
	Namespace = "obfuscator",
	Version = "1.0"
)]

[assembly:AddinName ("obfuscator")]
[assembly:AddinCategory ("obfuscator")]
[assembly:AddinDescription ("obfuscator")]
[assembly:AddinAuthor ("jawad")]

[assembly:AddinDependency ("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly:AddinDependency ("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]
