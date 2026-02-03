#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

"${script_dir}/run-rabbitmq.sh"

repo_root="$(cd "${script_dir}/.." && pwd)"

dotnet run --project "${repo_root}/src/Project.Management.Api/Project.Management.Api.csproj"
