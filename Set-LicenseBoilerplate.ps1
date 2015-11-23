param($target = "c:\\tmp", $companyname = "Lab49")


# Read boilerplate text from text file
# Recursive loop through all files having given extension
# Calculate the extension-specific header
# If a file begins with the extension-specific header, skip
# If the file has the start and ending markers, replace them and what's in between
# Otherwise, a file lacks the first bit of the extension-specific header, so insert
# Repeat for other extensions

$header = "//-----------------------------------------------------------------------

// <copyright file=""{0}"" company=""{1}"">

//     Copyright (c) {1}. All rights reserved.

// </copyright>

//-----------------------------------------------------------------------`r`n"

function Write-Header ($file) {

    $content = Get-Content $file

    $filename = Split-Path -Leaf $file

    $fileheader = $header -f $filename,$companyname
    Set-Content $file $fileheader

    Add-Content $file $content

}

Get-ChildItem $target -Recurse | ? { $_.Extension -like ".txt" } | % {

    Write-Header $_.PSPath.Split(":",3)[2]

}