##############################################################################
##
## Set-LicenseBoilerplate
##
## adapted from method by Kishor Aher
## https://kishordaher.wordpress.com/2010/03/11/powershell-copyright-header-generator-script/
##
## used for code in GitHub repository
## https://github.com/jimkropa/misc.corlib
##
##############################################################################

<#
	.SYNOPSIS
		Read boilerplate text from text file
		Recursive loop through all files having given extension
		Calculate the extension-specific header
		If a file begins with the extension-specific header, skip
		If the file has the start and ending markers, replace them and what's in between
		Otherwise, a file lacks the first bit of the extension-specific header, so insert
		Repeat for other extensions
#>

param($target = $pwd, $headerFile = "license-boilerplate.txt")

$header = "Copyright (c) 2015 Jim Kropa (http://www.kropa.net/)

Licensed under the Apache License, Version 2.0 (the ""License"");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an ""AS IS"" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
"

function Write-Header ($file, $extensionSpecificHeader) {
	# Read the original contents of the file as a string
	$content = Get-Content $file

	# TODO: Determine whether to add the header or replace an existing header,
	# based on the first and last lines of the $extensionSpecificHeader

	# Isolate the filename
	$filename = Split-Path -Leaf $file

	# Send the isolated filename
	# as a parameter to the string formatter
	$fileheader = $extensionSpecificHeader -f $filename

	Set-Content $file $fileheader
	Add-Content $file $content
}

function Write-Headers ($extension, $extensionSpecificHeader) {
	# http://www.neolisk.com/techblog/powershell-specialcharactersandtokens
	#	| is of course piping output of one command to input of the next command
	#	? is "Output all items that conform with condition" like LINQ "WHERE"
	#	% is shorthand for "foreach"
	Get-ChildItem $target -Recurse | ? { $_.Extension -like $extension } | % {
		# The splitting and array index are to work around the odd behavior
		# of the "PSPath" property when iterating over files in a directory,
		# which is that the file path follows a prefix ending in two colons
		Write-Header $_.PSPath.Split(":",3)[2], $extensionSpecificHeader
	}
}

# Read the original contents of the file as a string
$header = Get-Content $headerFile;

$csHeader = "#region [ license and copyright boilerplate ]
/*" + $header + "*/
#endregion
"

Write-Headers (".cs", $csHeader)
