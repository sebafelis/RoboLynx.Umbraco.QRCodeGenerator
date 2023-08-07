    $current_version = '10.0.0'
    $regex_current_version = $current_version -replace '\.','\.'
    $changelog = Get-Content -Path .\CHANGELOG.md -Raw
    if ($changelog -match "##\s?\[Unreleased\]") 
    {
        $current_date = Get-Date -UFormat "%Y-%m-%d"
        $changelog -replace "##\s?\[Unreleased\].*","## [$current_version] - $current_date" | Set-Content -Path .\CHANGELOG.md

        Write-Host "Unreleased section in CHANGELOG.md was updated into current version [$current_version] - $current_date."
        Write-Host "##vso[task.setvariable variable=changelogUpdated]true"
    }
    elseif ($changelog -match "##\s?\[$regex_current_version\]")
    {
        Write-Host "##vso[task.LogIssue type=warning;]CHANGELOG.md already contains section with current version."
        Write-Host "##vso[task.setvariable variable=changelogUpdated]false"
    }
    else
    {
        Write-Host "##vso[task.LogIssue type=error;]CHANGELOG.md do not contains Unreleased section with new changes."
        Write-Host "##vso[task.setvariable variable=changelogUpdated]false"
        exit 1
    }