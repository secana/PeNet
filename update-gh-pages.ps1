Param(
    [Parameter(Mandatory=$true)]
    [string]$token
)

New-Item -Type Directory tmp
git clone https://azure-bot:$token@github.com/secana/PeNet.git .\tmp\PeNet --single-branch --branch gh-pages
Copy-Item -Recurse -Force .\docs\* .\tmp\PeNet\
Set-Location .\tmp\PeNet
git remote rm origin
git remote add origin https://azure-bot:$token@github.com/secana/PeNet.git
git config user.email azure-bot@penet.org
git config user.name "Azure Bot"
git add .
git commit -m "Update documentation"
git push --set-upstream origin gh-pages