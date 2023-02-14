$current_version = '10.0.0'
$regex_current_version = $current_version -replace '\.','\.'
    $regex_pattern = "(?s)(?:\n##\s\[$regex_current_version\].*?\r\n(?:\s*\r\n)*)(?<content>.*?)(?:\r\n##\s\[\d+\.\d+\.\d+].*?\r\n|\s*\Z)"

    $changelog = Get-Content -Path .\CHANGELOG.md -Raw
    if ($changelog -match "##\s?\[$regex_current_version\]") 
    {
        $changes = $changelog | Select-String -Pattern $regex_pattern
        $releaseNotes = $changes.Matches[0].Groups['content'] -replace "\r\n","%0D%0A"
        Write-Host "##vso[task.setvariable variable=releaseNotes]$releaseNotes"
        Write-Host "Extracted release note:"
        Write-Host "$releaseNotes"
    }
    else 
    {
        Write-Host "##vso[task.LogIssue type=error;]CHANGELOG.md don't contains section for current version."
    }