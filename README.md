# SK-Dev

Minimal console app demonstrating Microsoft Semantic Kernel + Azure OpenAI connector.

## Prerequisites
- .NET 8 SDK installed
- dotnet CLI (for user-secrets) or the ability to set environment variables

## Configuration (recommended)
Do NOT commit secrets. Use one of these approaches:

Local development (recommended)
1. Use dotnet user-secrets:

in project folder
dotnet user-secrets init 
dotnet user-secrets set "OPENAI_ENDPOINT" "https://your-endpoint" 
dotnet user-secrets set "OPENAI_API_KEY" "your_api_key" 
dotnet user-secrets set "OPENAI_DEPLOYMENT" "gpt-4.1-mini"

2. Or set session environment variables:
- PowerShell:
$env:OPENAI_ENDPOINT="https://your-endpoint" 
$env:OPENAI_API_KEY="your_api_key" 
$env:OPENAI_DEPLOYMENT="gpt-4.1-mini" dotnet run

- Bash:
export OPENAI_ENDPOINT="https://your-endpoint" 
export OPENAI_API_KEY="your_api_key" 
export OPENAI_DEPLOYMENT="gpt-4.1-mini" dotnet run

3. Visual Studio debug (do not commit)
Edit `Properties/launchSettings.json` and add environment variables to your profile for debugging only.

## CI / Production
- Store secrets in your CI/CD or cloud provider secret store and inject them as environment variables.
- Example (GitHub Actions):
env: 
	- OPENAI_ENDPOINT: ${{ secrets.OPENAI_ENDPOINT }} 
	- OPENAI_API_KEY: ${{ secrets.OPENAI_API_KEY }} 
	- OPENAI_DEPLOYMENT: ${{ secrets.OPENAI_DEPLOYMENT }}


## Security notes
- Rotate any keys that were previously committed.
- Add secrets and local secret files to `.gitignore`.
- Never log secret values.
- Prefer managed secret stores (Azure Key Vault, AWS Secrets Manager) in production.

## Helpful commands
- List user secrets:

## Build & Run

