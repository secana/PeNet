Param(
    [Parameter(Mandatory=$true)]
    [string]$token
)

$url = "https://azure-bot:"+$token+"@github.com/secana/PeNet.git"
New-Item -Type Directory tmp
git clone $url .\tmp\PeNet --single-branch --branch gh-pages
Copy-Item -Recurse -Force .\docs\* .\tmp\PeNet\
Set-Location .\tmp\PeNet
git remote rm origin
git remote add origin $url
git config user.email azure-bot@penet.org
git config user.name "Azure Bot"
git add .
git commit -m "Update documentation"
git push --set-upstream origin gh-pages