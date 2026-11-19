# Eternal Kingdoms — Foundation Build

Primera base jugable de interfaz para un MOBA de fantasía 5v5 en Unity, orientada a Android en horizontal.

## Qué incluye
- Menú principal y navegación funcional entre Jugar, Héroes, Tienda, Colección, Perfil, Eventos y Configuración.
- Sala de espera visual 5 vs 5 con espacios de jugadores de ejemplo.
- Perfil, tienda, colección, eventos y configuración con datos de demostración.
- UI creada por código para evitar depender de paquetes externos o assets de pago.
- Script de Editor para generar y guardar una escena inicial.
- Estructura modular para ampliar en fases posteriores.

## Requisitos
- Unity 2022.3 LTS o superior.
- Módulo Android Build Support si se desea compilar para Android.

## Cómo abrir
1. Descomprime el ZIP.
2. En Unity Hub, selecciona **Add > Add project from disk** y elige esta carpeta.
3. Abre el proyecto y espera la importación.
4. En Unity, usa **Eternal Kingdoms > Create Demo Scene**.
5. Abre `Assets/Scenes/EternalKingdomsDemo.unity` y pulsa Play.

También se incluye un arranque automático de respaldo para que la interfaz aparezca al entrar en Play aunque no se haya creado la escena.

## Android
- En `File > Build Settings` selecciona Android y cambia la orientación a Landscape desde Player Settings.
- La interfaz usa Canvas Scaler en modo Scale With Screen Size.
- Esta entrega es una base de interfaz; no contiene aún gameplay 3D, matchmaking, Supabase ni combate online.

## Controles
Todos los botones de navegación son funcionales dentro del prototipo. La configuración permite cambiar volumen y calidad visual de demostración.

## Estado honesto
- Implementado: interfaz y navegación local, contenido de ejemplo.
- Pendiente: arte final, modelos 3D, animaciones de héroes, backend, autenticación, economía real, combate y servidor multijugador.
