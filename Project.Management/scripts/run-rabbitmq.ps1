#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"

$existing = docker ps -a --format "{{.Names}}" | Where-Object { $_ -eq "rabbitmq" }

if ($existing) {
    docker start rabbitmq | Out-Null
} else {
    docker run -d --name rabbitmq `
        -p 5672:5672 -p 15672:15672 `
        rabbitmq:3-management | Out-Null
}
