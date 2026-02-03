#!/usr/bin/env bash
set -euo pipefail

if docker ps -a --format '{{.Names}}' | grep -qx 'rabbitmq'; then
  docker start rabbitmq >/dev/null
else
  docker run -d --name rabbitmq \
    -p 5672:5672 -p 15672:15672 \
    rabbitmq:3-management
fi
