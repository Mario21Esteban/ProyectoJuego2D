# ProyectoJuego2D - Track & Field: 100 Meter Dash

## 📋 Descripción General

**ProyectoJuego2D** es una adaptación del clásico juego **Track & Field** de Konami, específicamente la mecánica de **100 Meter Dash**. Es un juego de carreras 2D en tiempo real donde el jugador debe correr hacia la meta en el menor tiempo posible mientras evita obstáculos y recolecta bonificaciones.

### Objetivo Principal
- **Completar la carrera en el menor tiempo posible**
- Evitar obstáculos que ralentizan el progreso
- Recolectar las 3 estrellas bonus distribuidas en el recorrido
- Conseguir la mejor medalla: Oro (≤30s), Plata (31-60s) o Bronce (>60s)

---

## 🎮 Mecánica de Juego

### Sistema de Movimiento
- **Movimiento Horizontal**: A/D o Flechas Izq/Der para esquivar
- **Correr**: Shift + Movimiento para aumentar velocidad
  - Velocidad Normal: 4 unidades/segundo
  - Velocidad Sprint: 7 unidades/segundo
- **Saltar**: Espacio, W o Flecha Arriba (para esquivar obstáculos altos)

### Sistema de Vidas
- **Vidas Iniciales**: 3 corazones
- **Pérdida de Vida**: Al tocar un obstáculo (tropiezo)
- **Penalización por Tropiezo**: 
  - -1 vida
  - Ralentización a 1 u/s durante 2 segundos
  - Animación de golpe
- **Game Over**: Cuando vidas llegan a 0 → Reinicia escena

### Sistema de Tiempo y Puntuación
El jugador debe cruzar la meta dentro del tiempo límite para conseguir medallas:

| Medalla | Tiempo | Puntos |
|---------|--------|--------|
| 🥇 Oro | ≤ 30 segundos | 1000 |
| 🥈 Plata | 31-60 segundos | 500 |
| 🥉 Bronce | > 60 segundos | 100 |

### Sistema de Bonificaciones
- **Estrellas**: 3 bonus distribuidas en el nivel
- **Requisito**: Debe recolectar todas las 3 estrellas para poder completar el nivel
- **Pickup**: Se recolectan automáticamente al tocar

### Condición de Victoria
Llegar a la meta con:
- ✓ Al menos 1 vida
- ✓ Las 3 estrellas recolectadas
- ✓ Tiempo registrado

---

## 🏗️ Arquitectura del Proyecto

### Estructura de Carpetas

```
Assets/
├── SCRIPTS/                    # Código fuente del juego
│   ├── jugador.cs            # PlayerController - Control del personaje
│   ├── CollectibleManager.cs  # GameManager - Gestión del juego
│   ├── HUDManager.cs          # UI - Interfaz de usuario
│   ├── Coin.cs               # Estrella - Sistema de recoleccionables
│   ├── Obstaculo.cs          # Sistema de obstáculos
│   ├── Meta.cs               # Punto de llegada/meta
│   └── CameraFollow.cs       # Cámara - Seguimiento del jugador
├── RECURSOS/
│   ├── SPRITES/              # Personajes y animaciones
│   │   ├── fantasy-chibi-female-sprites-pixel-art/
│   │   └── fantasy-chibi-male-sprites-pixel-art/
│   ├── FONDOS/               # Fondos del nivel
│   │   ├── post-apocalyptic-pixel-art-game-backgrounds/
│   │   └── vampires-locations-battle-background-pack/
│   └── VARIOS/               # Otros recursos
├── Prefabs/                  # Prefabs reutilizables
│   ├── Coin.prefab          # Prefab de estrella/bonus
│   ├── Obstaculo.prefab     # Prefab de obstáculo
│   └── Plataforma.prefab    # Prefab de plataformas
├── Scenes/
│   └── SampleScene.unity    # Escena principal del juego
└── Settings/                # Configuración del proyecto
```

---

## 💻 Descripción de Scripts

### 1. **PlayerController.cs** (`jugador.cs`)
**Responsabilidad**: Controlar el movimiento y animaciones del jugador

**Componentes Principales**:
- Input handling (movimiento, salto, sprint)
- Física de movimiento con Rigidbody2D
- Detección de suelo con OverlapCircle
- Sistema de animaciones
- Volteo visual (flipX) según dirección

**Variables Clave**:
- `walkSpeed = 4f` - Velocidad de movimiento normal
- `runSpeed = 7f` - Velocidad de sprint
- `jumpForce = 10f` - Fuerza del salto
- `velocidadPenalizada = 1f` - Velocidad durante penalización
- `duracionPenalizacion = 2f` - Duración de penalización por obstáculo

**Métodos Públicos**:
- `RecibirGolpe()` - Recibe daño de obstáculo
- `ActivarMuerte()` - Activa animación de muerte

---

### 2. **GameManager.cs** (`CollectibleManager.cs`)
**Responsabilidad**: Gestionar estado global del juego

**Características**:
- Singleton pattern (`Instance`)
- Seguimiento de estrellas y vidas
- Cronómetro de la carrera
- Cálculo de puntuación por tiempo
- Control de fin de partida

**Variables Clave**:
- `estrellasTotal = 3` - Estrellas a recolectar
- `vidaMaxima = 3` - Vidas iniciales
- Rangos de tiempo para medallas (30s, 60s)
- Puntuación base (Oro: 1000, Plata: 500, Bronce: 100)

**Métodos Principales**:
- `AgregarEstrella()` - Suma estrella recolectada
- `TieneSuficientesEstrellas()` - Verifica si puede completar
- `CalcularPuntuacion()` - Calcula puntos según tiempo
- `RecibirDanio()` - Reduce vida al golpe
- `DetenerCronometro()` - Pausa el contador al fin

---

### 3. **HUDManager.cs**
**Responsabilidad**: Gestionar interfaz de usuario

**Elementos en Pantalla**:
- Tiempo transcurrido (formato MM:SS)
- Contador de estrellas (X / 3)
- Barra visual de vidas (corazones)

**Características**:
- Singleton pattern (`Instance`)
- Actualización en tiempo real
- Sistema de corazones con Fill visual

---

### 4. **Coin.cs** (Estrella)
**Responsabilidad**: Sistema de coleccionables

**Funcionamiento**:
- OnTriggerEnter2D detecta contacto
- Suma al contador de estrellas vía GameManager
- Se destruye al ser recolectado

---

### 5. **Obstaculo.cs**
**Responsabilidad**: Obstáculos en la carrera

**Funcionamiento**:
- OnTriggerEnter2D detecta contacto con jugador
- Llama a `RecibirGolpe()` del jugador
- Causa tropiezo (penalización de 2 segundos)

---

### 6. **Meta.cs**
**Responsabilidad**: Punto de llegada del nivel

**Lógica**:
- Comprueba si jugador tiene suficientes estrellas
- Si Sí: Detiene cronómetro, calcula puntuación
- Si No: Muestra mensaje "¡Necesitas más estrellas!"
- Reinicia escena al completar

---

### 7. **CameraFollow.cs**
**Responsabilidad**: Seguimiento de cámara

**Características**:
- Sigue al jugador en eje X
- Límites horizontales: 0 a 56 unidades
- Se mantiene en Y y Z fijos para vista lateral

---

## 🎨 Assets y Recursos

### Sprites del Personaje
- **fantasy-chibi-female-sprites-pixel-art**: Personaje femenino Chibi
- **fantasy-chibi-male-sprites-pixel-art**: Personaje masculino Chibi
- Ambos en formato pixel art

### Fondos Disponibles
- **post-apocalyptic-pixel-art-game-backgrounds**: Ambiente post-apocalíptico
- **vampires-locations-battle-background-pack**: Ambiente de vampiros

### Animaciones
El personaje tiene las siguientes animaciones:
- `Speed` (float) - Velocidad de movimiento
- `IsRunning` (bool) - Estado de sprint
- `IsGrounded` (bool) - Contacto con suelo
- `isHit` (bool) - Animación de golpe/tropiezo
- `isDead` (bool) - Animación de muerte

---

## 🕹️ Controles del Juego

| Acción | Teclas |
|--------|--------|
| Mover Izquierda | A, ← |
| Mover Derecha | D, → |
| Saltar | Espacio, W, ↑ |
| Correr | Shift + Movimiento |
| Reiniciar | R (después de perder) |

---

## 🔄 Flujo de Juego Completo

```
1. INICIO
   └─ Se carga la escena SampleScene
   └─ GameManager se inicializa
   └─ Cronómetro comienza

2. JUEGO EN CURSO
   ├─ Jugador corre evitando obstáculos
   ├─ Puede saltar sobre obstáculos
   ├─ Recolecta estrellas bonus
   └─ HUD muestra progreso en tiempo real

3. EVENTOS POSIBLES
   a) TROPIEZO (obstáculo)
      └─ Penalización 2 segundos
      └─ Pierde 1 vida
      └─ Animación de golpe
      
   b) RECOLECCIÓN (estrella)
      └─ Suma al contador
      └─ Se destruye el bonus

   c) MUERTE (vida = 0)
      └─ Animación de muerte
      └─ Reinicia escena

4. FIN - LLEGA A META
   ├─ Verifica: ¿Tiene 3 estrellas?
   ├─ SI → Calcula medalla y puntos
   ├─ NO → Muestra "¡Necesitas más estrellas!"
   └─ Reinicia escena

5. PUNTUACIÓN
   ├─ Tiempo ≤ 30s → ORO (1000 pts)
   ├─ Tiempo 31-60s → PLATA (500 pts)
   └─ Tiempo > 60s → BRONCE (100 pts)
```

---

## 🐛 Estado Actual y Notas de Desarrollo

### Completado
- ✅ Sistema de movimiento y control
- ✅ Física de salto y detección de suelo
- ✅ Sistema de obstáculos y penalizaciones
- ✅ Recolección de estrellas
- ✅ Sistema de vidas
- ✅ Cronómetro y cálculo de puntuación
- ✅ UI (HUD con tiempo, estrellas, corazones)
- ✅ Cámara con seguimiento
- ✅ Animaciones básicas

### Notas de Implementación
- Usa **Singleton Pattern** para GameManager y HUDManager
- **Requiere componentes**: Rigidbody2D, Animator, SpriteRenderer en jugador
- **Detección de colisiones**: OnTriggerEnter2D para recolecci
ones y obstáculos
- **Sistema de física**: Gravity Scale y linearVelocity controlados manualmente

---

## 📊 Parámetros Configurables

Todos estos valores están configurados en el Inspector de Unity y pueden ajustarse:

```csharp
// PlayerController
walkSpeed = 4f                  // Velocidad base
runSpeed = 7f                   // Velocidad sprint
jumpForce = 10f                 // Altura del salto
velocidadPenalizada = 1f       // Velocidad en penalización
duracionPenalizacion = 2f      // Duración penalización (segundos)

// GameManager
estrellasTotal = 3              // Estrellas a recolectar
vidaMaxima = 3                  // Vidas iniciales
tiempoOroPuntos = 30f          // Tiempo para oro (segundos)
tiempoPlata = 60f              // Tiempo para plata (segundos)
puntosOro = 1000               // Puntos medalla oro
puntosPlata = 500              // Puntos medalla plata
puntosBronce = 100             // Puntos medalla bronce

// CameraFollow
minX = 0f                       // Límite cámara izquierda
maxX = 56f                      // Límite cámara derecha
```

---

## 🎯 Objetivo del Proyecto

Este es un proyecto educativo/recreativo que recrea la experiencia clásica de **Track & Field - 100 Meter Dash**, adaptado a un juego 2D moderno en Unity con:
- Mecánica de carreras contra reloj
- Sistema de obstáculos dinámicos
- Bonificaciones coleccionables
- Sistema de medallas basado en tiempo
- Gráficos pixel art chibi

---

## 📝 Autor y Fecha

- **Proyecto**: ProyectoJuego2D
- **Motor**: Unity
- **Lenguaje**: C#
- **Últimas actualizaciones**: Abril 2026

---


