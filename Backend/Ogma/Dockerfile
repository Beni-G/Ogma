# ==========================================
# Stage 1: Build & Optimize Keycloak
# ==========================================
FROM quay.io/keycloak/keycloak:26.1 as builder

# Enable health and metrics endpoints for monitoring/Docker healthchecks
ENV KC_HEALTH_ENABLED=true
ENV KC_METRICS_ENABLED=true

# Specify your production database provider (e.g., postgres, mysql, mssql)
ENV KC_DB=postgres

WORKDIR /opt/keycloak

# This command pre-compiles and optimizes the Keycloak runtime configuration
RUN /opt/keycloak/bin/kc.sh build

# ==========================================
# Stage 2: Clean Runtime Image
# ==========================================
FROM quay.io/keycloak/keycloak:26.1

# Copy the optimized build artifacts from the builder stage
COPY --from=builder /opt/keycloak/ /opt/keycloak/

# Define default runtime variables (These can be overridden later in Docker Compose or Kubernetes)
ENV KC_DB=postgres
ENV KC_HTTP_ENABLED=true

# Inform Docker that the container listens on port 8080
EXPOSE 8080

# The "--optimized" flag tells Keycloak to skip the build steps and launch instantly
ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start", "--optimized"]