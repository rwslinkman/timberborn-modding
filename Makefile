COMPOSE_FILE=timberborn-prometheus-exporter.compose.yaml

.PHONY: up down logs restart

up:
	docker compose -f $(COMPOSE_FILE) up -d

down:
	docker compose -f $(COMPOSE_FILE) down

restart: down up