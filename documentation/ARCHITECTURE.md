# Arquitectura inicial

## Cliente
Unity 2022.3 LTS o superior. La interfaz está implementada con Unity UI (uGUI) y se construye por código en `EternalKingdomsBootstrap`.

## Módulos previstos
- UI: navegación, lobby, perfil, tienda, colección y configuración.
- Characters: catálogo de héroes y skins (siguiente fase).
- Gameplay: movimiento, combate, habilidades y objetivos (futuro).
- Networking: cliente de salas y comunicación (futuro).
- Backend: Supabase Auth, Database y Storage (futuro).

## Decisión de red futura
El servidor de partida deberá ser autoritativo. Supabase se utilizará para identidad, perfiles, inventario persistente y metadatos; no como autoridad de simulación de combate.
