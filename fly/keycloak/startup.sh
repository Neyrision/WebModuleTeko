#!/bin/bash
if [ ! -f /opt/keycloak/data/.optimized ]; then
    echo "Running initial keycloak setup..."
    touch /opt/keycloak/data/.optimized
    /opt/keycloak/bin/kc.sh start --hostname $KC_HOSTNAME --proxy-headers xforwarded
else
    echo "Running keycloak optimized"
    /opt/keycloak/bin/kc.sh start --optimized --hostname $KC_HOSTNAME --proxy-headers xforwarded
fi