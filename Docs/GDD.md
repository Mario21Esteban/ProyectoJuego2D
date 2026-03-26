# GAME DESIGN DOCUMENT (GDD)
## Proyecto: Runner 2D Academia

**Versión**: 1.0  
**Fecha de creación**: 24 de marzo de 2026  
**Estado**: MVP Specification  
**Notas**: Documento vivo — sujeto a revisiones iterativas

---

## 1. VISIÓN GENERAL

### 1.1 Información Básica

| Aspecto | Descripción |
|---------|------------|
| **Título del Juego** | *[Por definir]* — Runner (Excitebike-style) |
| **Género** | Runner 2D por niveles / Arcade Racing |
| **Plataforma Principal** | Windows (PC) via Unity Editor / Build Standalone |
| **Plataformas Secundarias** | WebGL (futuro) |
| **Engine** | Unity 2022 LTS+ |
| **Lenguaje de Programación** | C# |
| **Target Audience** | Académico (proyecto de clase) |
| **Clasificación de Contenido** | E para Todos (sin violencia, sin contenido inapropiado) |

### 1.2 Concepto de Una Línea

**"Un piloto novato compite en emocionantes carreras por niveles, ganando experiencia y superando rivales progresivamente más difíciles para convertirse en el campeón de la liga."**

### 1.3 Elevator Pitch (30 segundos)

Juego de carreras arcade 2D estilo Excitebike donde controlas un piloto novato. Avanzas automáticamente hacia la derecha, saltas para evitar obstáculos y compites contra un rival único por nivel. Gana la carrera llegando primero a la meta. Derrota al campeón en el nivel final y completa tu camino a la gloria.

---

## 2. DESCRIPCIÓN DEL JUEGO

### 2.1 Loop de Gameplay Principal

```
┌────────────────────────────────┐
│   INICIO DEL NIVEL             │
│  (Pantalla de narrativa)       │
└────────────────────────────────┘
           ↓
┌────────────────────────────────┐
│   GAMEPLAY ACTIVO              │
│  ┌──────────────────────────┐  │
│  │ Movimiento automático    │  │
│  │ Salto mediante Input     │  │
│  │ Rival se mueve autónomo  │  │
│  │ Evitar obstáculos        │  │
│  │ Avanzar hacia meta       │  │
│  └──────────────────────────┘  │
└────────────────────────────────┘
           ↓
    ┌──────┴──────┐
    │             │
    ↓             ↓
┌─────────┐  ┌──────────┐
│ VICTORIA│  │ DERROTA  │
│ (Meta 1º)  │(J2 llega,*cae,
│         │  │ toca obs.) │
└─────────┘  └──────────┘
    ↓             ↓
┌─────────────────────────────────┐
│   RESULTADO & PROGRESIÓN        │
│  [SIGUIENTE NIVEL] [REINTENTAR] │
└─────────────────────────────────┘
```

### 2.2 Flujo de Juego Completo

1. **Main Menu** → Seleccionar iniciar
2. **Intro del Nivel** → Presentar rival y narrativa (pantalla de transición)
3. **Gameplay** ← **[Loop de 15-30 segundos]**
   - Avance automático + Input de salto
   - Rival progresa automáticamente
   - Obstáculos y terreno
4. **Pantalla de Resultado**
   - Si Victoria: "¡Ganaste! Avanza"
   - Si Derrota: "Perdiste. Reintentar o Menú"
5. **Progresión** → Siguiente nivel o Game Over

### 2.3 Duración Estimada

- **Partida promedio**: 20 segundos (gameplay puro)
- **Con cinemáticas de intro**: 25 segundos
- **MVP Completo (6 niveles)**: 5-10 minutos end-to-end
- **Tiempo total de desarrollo**: 2-4 semanas (según recursos)

---

## 3. MECÁNICAS PRINCIPALES

### 3.1 Movimiento del Jugador

#### Movimiento Horizontal (Automático)
```
Especificación:
- Velocidad: 5.0 unidades/segundo (constante)
- Dirección: Siempre hacia la derecha (Vector2.right)
- Control: NINGUNO — es automático
- Implementación: En FixedUpdate(), aplicar velocidad al Rigidbody2D
```

**Comportamiento:**
- El jugador se mueve sin interacción del usuario
- Permite al jugador enfocarse en saltar
- Crea sentido de urgencia

#### Salto (Controlado por Input)
```
Especificación:
- Input: Tecla Space (Input.GetKeyDown(KeyCode.Space))
- Velocidad vertical: Impulso de 5.0 unidades/s (jumpForce)
- Altura máxima: ~1.8 unidades
- Duración en aire: ~0.6 segundos
- Gravedad: -9.81 (estándar Physics2D)
- Restricción: Solo se puede saltar estando en el suelo
```

**Comportamiento:**
```csharp
// Pseudocódigo
void Update() {
  if(Input.GetKeyDown(KeyCode.Space) && isGrounded) {
    Jump();
  }
}

void Jump() {
  rigidbody.velocity = new Vector2(
    rigidbody.velocity.x,  // Mantener velocidad horizontal
    jumpForce              // Aplicar impulso vertical
  );
}
```

#### Detección de Suelo (Ground Check)
```
Especificación:
- Método: Raycast downward desde posición del jugador
- Distancia: 0.1 unidades bajo los pies
- Layer: "Terrain" (colisionables)
- Alternativa: OverlapCircle con radio 0.2
```

**Comportamiento:**
- Se ejecuta cada frame en Update()
- Define si el jugador puede saltar
- Evita saltos infinitos en el aire

#### Drag (Fricción)
```
Especificación:
- Ground Drag: 1.0 (menor movimiento en suelo)
- Air Drag: 0.5 (mayor libertad en aire)
- Implementación: Modular Rigidbody2D.drag según estado
```

### 3.2 Entorno (Terreno)

#### Tipos de Terreno

| Tipo | Descripción | Física | Reto |
|------|-------------|--------|------|
| **Ground (Base)** | Terreno plano principal | Static Collider | Ninguno — atravesable |
| **Plataforma** | Elevada, requiere salto | Static Collider | Timing del salto |
| **Hueco** | Gap sin terreno | No hay collider | Debes saltar sobre el vacío |
| **Rampa** | Inclinada 30-45° | PolygonCollider2D | Subir/bajar sin perder control |
| **Obstáculo Dañino** | Pinchos, rocas | Trigger + Script | Derrota instantánea |

#### Layout de Terreno Típico

```
         ┌──┐
         │  │ (Plataforma)
┌─────┐  │  │┌──┐
│     │  │  ││  │
│     │┌─┘  ││  │┌─────── META
│ ││  ││ Gap││  ││
└─┴──┴┘    └──┘└─────
(Ground + obstáculo)
```

### 3.3 Sistema de Carrera (1 vs 1)

#### Rival como Competencia
```
Especificación:
- Cantidad: 1 rival por nivel
- Velocidad: 80-90% de la velocidad del jugador
  (Para permitir que el jugador gane con buen timing)
- IA: Automática (sin pathfinding avanzado)
- Sincronización: Inician al mismo tiempo
```

#### Condiciones de Victoria

| Participante | Condición de Victoria |
|--------------|----------------------|
| **Jugador** | Cruza First la línea de meta (Trigger) |
| **Rival** | Cruza primero la meta → Jugador pierde |

### 3.4 Condiciones de Derrota

```
1. Rival llega primero a la meta
   → Pantalla "DERROTA — Rival fue más rápido"

2. Jugador cae fuera de bounds
   → Condición: Y < -20 unidades
   → Pantalla "DERROTA — ¡Caíste!"

3. Jugador toca obstáculo dañino
   → Condición: Colisión con trigger "Obstacle"
   → Pantalla "DERROTA — ¡Impacto!"
```

---

## 4. NARRATIVA & PROGRESIÓN

### 4.1 Trama Principal

#### Acto 1: Origen (Niveles 1-2)
- El protagonista es un piloto novato
- Comienza a entrenar
- Compite contra amigos y conocidos

#### Acto 2: Escalada (Niveles 3-4)
- Se gana reputación
- Rivalry se intensifica
- Encuentros con pilotos legendarios

#### Acto 3: Clímax (Niveles 5-6)
- Enfrentamiento final contra el Campeón Actual
- ¿Podrás ser leyenda?

### 4.2 Personajes & Rivales

#### Protagonista
- **Nombre**: [Por definir - el jugador eres tú]
- **Rol**: Piloto novato ambicioso
- **Motivación**: Ser el mejor
- **Edad**: 16-18 años
- **Personalidad**: Determinado, aprendiz

#### Rival 01 — "El Amigo"
- **Nombre**: Carlos
- **Nivel**: Principiante
- **Dificultad**: ⭐ (Fácil)
- **Contexto**: Tu mejor amigo presenta a la competencia
- **Narrativa Intro**: "Tu mejor amigo Carlo decide competir contigo. ¿Podrás con él?"
- **Narrativa Victoria**: "¡Ganaste! Carlos te anima a continuar."

#### Rival 02 — "La Técnica"
- **Nombre**: Ana
- **Nivel**: Intermedio
- **Dificultad**: ⭐⭐ (Medio)
- **Contexto**: Piloto técnicamente superior
- **Narrativa Intro**: "Ana, una piloto técnicamente brillante, entra en pista. ¿Tienes lo necesario?"
- **Narrativa Victoria**: "Demostraste tu valía contra Ana. ¡Avanza!"

#### Rival 03 — "El Veterano"
- **Nombre**: Luis
- **Nivel**: Avanzado
- **Dificultad**: ⭐⭐⭐ (Difícil)
- **Contexto**: Piloto experimentado con años en carreras
- **Narrativa Intro**: "Luis, un veterano con 10 años de experiencia, se burla de tus intentos. Demuéstrale que está equivocado."

#### Rival 04 — "La Rival Local"
- **Nombre**: Sofía
- **Nivel**: Experto
- **Dificultad**: ⭐⭐⭐⭐ (Muy Difícil)
- **Contexto**: Tu rival de infancia, ahora campeón regional
- **Narrativa Intro**: "Sofía, tu rival desde la niñez y campeona regional, baja a pista. Esta es tu oportunidad de reivindicación."

#### Rival 05 — "El Ídolo"
- **Nombre**: Mario
- **Nivel**: Pro
- **Dificultad**: ⭐⭐⭐⭐⭐ (Muy Muy Difícil)
- **Contexto**: piloto legendario, inspiración del protagonista
- **Narrativa Intro**: "Mario, tu ídolo y triple campeón, acepta tu desafío. Esta es tu prueba final antes de la gloria."

#### Rival 06 — "El Campeón" (BOSS)
- **Nombre**: Unidad
- **Nivel**: Maestro
- **Dificultad**: ⭐⭐⭐⭐⭐⭐ (Imposible sin perfección)
- **Contexto**: El legendario campeón invicto que nadie ha batido
- **Narrativa Intro**: "UNIDAD, el campeón invicto por 5 años, sale a la pista. Si le ganas, eres LEYENDA."
- **Narrativa Victoria**: "¡LO HICISTE! ¡ERES EL NUEVO CAMPEÓN! ¡LEYENDA ETERNA!"

### 4.3 Estructura Narrativa de Niveles

```
NIVEL 01 — Carlos (Amigo)      → Victoria → NIVEL 02
NIVEL 02 — Ana (Técnica)       → Victoria → NIVEL 03
NIVEL 03 — Luis (Veterano)     → Victoria → NIVEL 04
NIVEL 04 — Sofía (Rival Local) → Victoria → NIVEL 05
NIVEL 05 — Mario (Ídolo)       → Victoria → NIVEL 06 (FINAL)
NIVEL 06 — UNIDAD (Campeón)    → Victoria → Créditos / Fin
```

### 4.4 Implementación de Narrativa

#### Formato de Texto Narrativo
```json
{
  "level_1": {
    "intro": "Tu mejor amigo Carlos decide competir contigo. ¿Podrás ganar?",
    "rival_name": "Carlos",
    "victory": "¡Ganaste! Carlos te anima a continuar.",
    "defeat": "Carlos te gana esta vez. ¡Reintentar!"
  },
  "level_2": {
    "intro": "Ana, una piloto técnica, entra en pista. ¿Tienes lo necesario?",
    "rival_name": "Ana",
    "victory": "Demostraste tu valía. ¡Avanza!",
    "defeat": "Ana es muy rápida. ¡Reintentar!"
  }
  // ... (más niveles)
}
```

#### UI Narrativa
- **Pre-Nivel**: Pantalla de intro (2 segundos) con texto de rival
- **Gameplay**: Nombre del rival visible en esquina superior
- **Post-Nivel**: Resultado con narrativa contextual

**NO se incluyen:**
- Cinemáticas renderizadas
- Voiceover/Audio narrativo
- Secuencias complejas

---

## 5. GAMEFLOW & MENÚS

### 5.1 Diagrama de Flujo

```
┌─────────────────┐
│   SPLASH LOGO   │ (1 segundos)
└────────┬────────┘
         ↓
┌─────────────────────────────┐
│      MAIN MENU              │
│  [JUGAR]  [INSTRUCCIONES]   │
│  [CRÉDITOS]  [SALIR]        │
└────────┬────────────┬───────┘
         │            │
         ↓            ↓
   [JUGAR]    [Ver Info / Volver]
         │
         ↓
┌──────────────────────┐
│  LEVEL SELECT (MVP)  │ (Opcional MVP)
│  [Nivel 01]          │    O
│  [Nivel 02] [Bloqueado]    Comienza directamente Nivel 01
│  ...
└────────┬─────────────┘
         │
         ↓
┌──────────────────────────────┐
│   INTRO LEVEL SCREEN         │
│  "NIVEL 01 — Carlos"         │
│  "Tu mejor amigo compete..."│
│  [PRESIONA SPACE PARA EMPEZAR]│
└────────┬─────────────────────┘
         │
         ↓
┌──────────────────────────────┐ ←─┐
│   GAMEPLAY [AUTO-RUNNING]    │   │
│  Jugador salta, Rival compite │   │
│  Goal: Llegar a la meta      │   │
└────────┬─────────────────────┘   │
         │                         │
    ┌────┴────┐              ┌─────┴────┐
    ↓         ↓              │ [REINTENTAR]
┌─────────┐┌──────────┐      │
│Victoria ││ Derrota  │      │
└────┬────┘└────┬─────┘      │
     │           └────────────┘
     │
     ↓
┌──────────────────────────┐
│  RESULTADO SCREEN        │
│  ¡Ganaste contra Carlos! │
│  [SIGUIENTE NIVEL]       │
│  [REINTENTAR]            │
│  [MENÚ]                  │
└──────┬───────────────────┘
       │
       ├─→ [SIGUIENTE] → NIVEL 02 INTRO
       ├─→ [REINTENTAR] → NIVEL 01 INTRO (Loop)
       └─→ [MENÚ] → MAIN MENU
```

### 5.2 Descripciones de Pantallas

| Pantalla | Duración | Controles | Elementos |
|----------|----------|-----------|-----------|
| **Splash** | 1-2s | Ninguno | Logo / Fade in-out |
| **Main Menu** | ∞ | Botones / Teclas | Background, 4 botones principales |
| **Intro Nivel** | 3s | Space para continuar | Nombre rival, narrativa, imagen rival |
| **Gameplay** | 15-30s | Space (salto) | Jugador, rival, terreno, UI minimalista |
| **Resultado** | ∞ | Botones | Título resultado, 3 opciones |

### 5.3 Instrucciones In-Game

```
PANTALLA DE INSTRUCCIONES (Accesible desde Main Menu)

═════════════════════════════════════════
           CÓMO JUGAR
═════════════════════════════════════════

OBJETIVO:
  Llega primero a la meta batiendo a tu rival.

CONTROLES:
  [ESPACIO] — Salta
  (El movimiento es automático)

TIPS:
  • Salta a tiempo para evitar obstáculos
  • No caigas fuera de la pista
  • Observa a tu rival — ¡pero no te distraigas!
  • Cada nivel es más difícil — ¡prepárate!

[ENTENDIDO] [VOLVER AL MENÚ]
```

---

## 6. ARTE & ESTÉTICA

### 6.1 Estilo Visual

| Aspecto | Descripción |
|---------|------------|
| **Género Artístico** | Pixel art retro / 2D plano minimalista |
| **Inspiración Visual** | Excitebike (NES), Flappy Bird, Geomatry Dash |
| **Perspectiva** | Vista lateral (side-view) |
| **Escala de Personaje** | ~1-2 unidades (cubos/rectángulos simples) |
| **Nivel de Detalle (MVP)** | Muy bajo — formas geométricas básicas |

### 6.2 Paleta de Colores

```
PRIMARIOS:
  Fondo: #1a1a2e (Azul oscuro)
  Ground: #16a34a (Verde)
  
PERSONAJES:
  Jugador: #3b82f6 (Azul)
  Rival: #dc2626 (Rojo)
  
OBJETOS:
  Obstáculos: #78350f (Marrón)
  Meta: #fbbf24 (Dorado)
  Peligro (Pinchos): #991b1b (Rojo oscuro)
  
UI:
  Texto Principal: #ffffff (Blanco)
  Acentos: #06b6d4 (Cyan)
```

### 6.3 Diseño de Personajes (Sprites Simples)

#### Jugador
```
┌─────┐
│ 🟦  │  Cuadrado azul con marcador
│ 🟦  │  (altura 1.5u, ancho 0.8u)
└─────┘
```

#### Rival
```
┌─────┐
│ 🟥  │  Cuadrado rojo con marcador
│ 🟥  │  (altura 1.5u, ancho 0.8u)
└─────┘
```

#### Obstáculos
```
Pinchos:   Pequeño triángulo rojo 🔺
Rampa:     Línea inclinada ╱
Plataforma: Rectángulo verde  ▬
```

### 6.4 Animation (MVP Mínima)

- **Jugador**: Sin animación (sprite estático)
- **Rival**: Sin animación (sprite estático)
- **Transiciones**: Fade in/out de pantallas (simple)
- **Efectos**: Ninguno (MVP)

---

## 7. MECÁNICA DETALLADA: IA DEL RIVAL

### 7.1 Algoritmo de Movimiento del Rival

```
PSEUDOCÓDIGO:

class RivalController {
  
  // Estado persistente
  private float decisionTimer = 0.5f;  // Cada cuántos segundos decide
  private float lastDecisionTime = 0f;
  
  // Configuración
  private float moveSpeed = 4.5f;      // 90% de jugador
  private float jumpForce = 4.5f;
  private float raycastDistance = 2.0f;
  
  void FixedUpdate() {
    // MOVIMIENTO HORIZONTAL (automático, como jugador)
    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
  }
  
  void Update() {
    // LÓGICA DE SALTO
    if (isGrounded && (Time.time - lastDecisionTime >= decisionTimer)) {
      lastDecisionTime = Time.time;
      
      // Verificar si hay obstáculo adelante (raycast)
      bool obstaculoDetectado = Physics2D.Raycast(
        transform.position,
        Vector2.right,
        raycastDistance,
        LayerMask.GetMask("Terrain")
      );
      
      if (obstaculoDetectado) {
        Jump();  // Saltar si detecta obstáculo
      } else {
        // 70% probabilidad de saltar, 30% de no saltar
        if (Random.value < 0.7f) {
          Jump();
        }
      }
    }
  }
  
  void Jump() {
    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
  }
  
  bool CheckGrounded() {
    // Raycast hacia abajo
    return Physics2D.OverlapCircle(
      transform.position + Vector3.down * 0.1f,
      0.2f,
      LayerMask.GetMask("Terrain")
    ) != null;
  }
}
```

### 7.2 Parámetros Ajustables por Nivel

| Nivel | Velocidad Rival | Jump Timer | Probabilidad Salto | Comportamiento |
|-------|-----------------|------------|-------------------|----------------|
| 1 (Carlos) | 4.0 u/s | 0.8s | 60% | Lento, predecible |
| 2 (Ana) | 4.5 u/s | 0.6s | 70% | Medio, ocacional error |
| 3 (Luis) | 4.7 u/s | 0.5s | 80% | Rápido, pocas faltas |
| 4 (Sofía) | 4.85 u/s | 0.4s | 85% | Muy rápido, experto |
| 5 (Mario) | 4.95 u/s | 0.3s | 90% | Casi perfecto |
| 6 (UNIDAD) | 5.0 u/s | 0.2s | 100% | Perfección absoluta |

### 7.3 Comportamiento Especial (Futuro)

```
Actualmente (MVP): No hay comportamiento especial.

Futuro posible:
- Si rival está MUY atrás, acelerar momentáneamente
- Si rival está adelante, saltar más frecuentemente
- Evitar patrones predecibles
- Reacción a velocidad del jugador
```

---

## 8. ESPECIFICACIONES TÉCNICAS

### 8.1 Configuración de Proyecto

| Parámetro | Valor |
|-----------|-------|
| **Motor** | Unity 2022 LTS o superior |
| **Lenguaje** | C# (.NET Standard 2.1) |
| **Resolución** | 1920x1080 (escalable a 16:9) |
| **Aspect Ratio** | 16:9 |
| **Color Space** | Linear o Gamma (recomendado: Linear) |
| **Target FPS** | 60 FPS |
| **Physics Engine** | Physics2D |
| **Render Pipeline** | Universal Render Pipeline (URP) |

### 8.2 Configuración de Físicas (Physics2D)

```csharp
[Physics2D Settings]
Gravity: 0, -9.81
Default Material: 
  - Friction: 0.4
  - Bounce: 0

Collision Matrix:
  Default    ↔ Default   = ✓
  Default    ↔ Player    = ✓
  Default    ↔ Rival     = ✓
  Player     ↔ Rival     = ✗ (no chocan entre sí)
  Player     ↔ Trigger   = ✓
  Rival      ↔ Trigger   = ✓
```

### 8.3 Arquitectura de Scripts

```
Assets/
├── Scripts/
│   ├── Player/
│   │   └── PlayerController.cs
│   │       ├── Move()
│   │       ├── Jump()
│   │       ├── CheckGrounded()
│   │       └── ApplyDrag()
│   │
│   ├── Rival/
│   │   └── RivalController.cs
│   │       ├── Move()
│   │       ├── DecideJump()
│   │       ├── Jump()
│   │       └── DetectObstacle()
│   │
│   ├── Game/
│   │   ├── GameManager.cs
│   │   │   ├── CheckVictory()
│   │   │   ├── CheckDefeat()
│   │   │   ├── EndGame()
│   │   │   └── State enum { Playing, PlayerWon, RivalWon }
│   │   │
│   │   └── LevelManager.cs
│   │       ├── LoadLevel()
│   │       ├── InitializeLevel()
│   │       └── BindMetaTrigger()
│   │
│   ├── UI/
│   │   └── UIManager.cs
│   │       ├── ShowResult()
│   │       ├── ShowIntro()
│   │       └── HandleUI()
│   │
│   └── Track/
│       └── TrackGenerator.cs (Opcional MVP)
│           ├── GenerateTrack()
│           └── SpawnObstacle()
│
├── Prefabs/
│   ├── Player.prefab
│   │   ├── Transform
│   │   ├── Rigidbody2D
│   │   ├── BoxCollider2D (Ground Check)
│   │   └── PlayerController.cs
│   │
│   ├── Rival.prefab
│   │   ├── Transform
│   │   ├── Rigidbody2D
│   │   ├── BoxCollider2D
│   │   └── RivalController.cs
│   │
│   └── Terrain/
│       ├── Ground.prefab
│       ├── Platform.prefab
│       ├── Rampa.prefab
│       └── Obstacle.prefab
│
├── Scenes/
│   ├── MainMenu.unity
│   ├── Level01.unity
│   ├── Level02.unity
│   └── ... (Niveles 3-6)
│
└── Resources/
    └── narrative.json
```

### 8.4 Componentes de GameObject

#### Player GameObject
```
GameObject: Player
├── Transform (position = 0, 0)
├── Rigidbody2D
│   ├── Body Type: Dynamic
│   ├── Mass: 1
│   ├── Gravity Scale: 1
│   ├── Constraints: Freeze Rotation Z
│   └── Collision Detection: Continuous
├── BoxCollider2D (Main)
│   ├── Size: 0.8 × 1.5
│   ├── Is Trigger: false
│   └── Layer: Player
├── BoxCollider2D (GroundCheck)
│   ├── Offset: (0, -0.8)
│   ├── Size: 0.6 × 0.1
│   ├── Is Trigger: true
│   └── Layer: GroundCheck
├── SpriteRenderer
│   ├── Sprite: PlayerSprite (simple quad azul)
│   └── Color: #3b82f6
└── PlayerController (Script)
    ├── moveSpeed = 5.0
    ├── jumpForce = 5.0
    ├── groundDrag = 1.0
    ├── airDrag = 0.5
    └── groundCheckRadius = 0.2
```

#### Rival GameObject
```
GameObject: Rival
├── Transform (position = 2, 0)
├── Rigidbody2D (igual a Player)
├── BoxCollider2D (Main)
├── BoxCollider2D (GroundCheck)
├── SpriteRenderer
│   └── Color: #dc2626 (Rojo)
└── RivalController (Script)
    ├── moveSpeed = 4.5
    ├── jumpForce = 4.5
    ├── decisionTimer = 0.5
    └── raycastDistance = 2.0
```

### 8.5 Capas (Layers)

```
Crear en Unity:
- Default (existente)
- Player
- Rival
- Terrain
- Obstacle (para pinchos / dañinos)
- Meta (trigger de victoria)
- DeathZone (trigger de derrota)
```

---

## 9. DESCRIPCIÓN DETALLADA DE NIVELES

### 9.1 Nivel 01 — "El Comienzo"

| Atributo | Valor |
|----------|-------|
| **Rival** | Carlos (Amigo) |
| **Dificultad** | ⭐ Fácil |
| **Velocidad Rival** | 4.0 u/s |
| **Longitud del Recorrido** | 80 unidades |
| **Duración Esperada** | 15 segundos |
| **Objetivo Narrativo** | Tutorial implícito de mecánica |

**Terreno:**
```
Ground plano: 0-80u
Plataforma 1: (20-25u, altura 1u) - Salto fácil
Plataforma 2: (45-48u, altura 2u) - Salto más alto
Hueco:        (60-62u, ancho 2u) - Primer salto crítico
Meta:         (80u, altura 0)
```

**Éxito del Nivel:**
- Jugador entiende cómo saltar
- Rival es fácil de batir
- Confianza para siguiente nivel

---

### 9.2 Nivel 02 — "La Técnica"

| Atributo | Valor |
|----------|-------|
| **Rival** | Ana |
| **Dificultad** | ⭐⭐ Medio |
| **Velocidad Rival** | 4.5 u/s |
| **Longitud del Recorrido** | 90 unidades |
| **Duración Esperada** | 18 segundos |
| **Objetivo Narrativo** | Introducir timing diferenciado |

**Terreno:**
```
Plataformas con gaps alternados
Rampa hacia arriba (30-38u)
Obstáculos (pinchos) en posiciones clave
Mayor variación de alturas
```

---

### 9.3 Nivel 03 a 06 — [Estructura Similar]

Cada nivel aumenta progresivamente:
- Velocidad del rival
- Complejidad del terreno
- Número y variedad de obstáculos
- Requiere timing más preciso

---

## 10. REQUISITOS NO-FUNCIONALES

### 10.1 Rendimiento

```
Target: 60 FPS consistente en PC estándar
Máximo de GameObjects activos: 50
Drawcalls por frame: <100
Memory: <512 MB para MVP completo
```

### 10.2 Accesibilidad (Futuro)

- Opción de colorblind mode
- Tamaño de texto ajustable
- Controles remappeables (futuro)

### 10.3 Plataformas Soportadas

**MVP:**
- Windows 10/11 (64-bit)

**Futuro:**
- macOS
- WebGL (Browser)
- Mobile (opcional)

---

## 11. RESTRICCIONES & LIMITACIONES

### 11.1 Alcance Deliberadamente Limitado

```
✗ Sin:
  - Animaciones complejas
  - Cinemáticas renderizadas
  - Assets externos profesionales
  - Sistema de poder-ups
  - Multijugador
  - Guardado de progreso persistente
  - Leaderboards
  - Sonido/Música
```

### 11.2 IMPORTANTE: MVP vs. Futuro

**Este GDD describe el MVP.** Las siguientes características se documentarán en GDD 2.0 si se aprueban:

- Más niveles (7+)
- Mejora de IA con state machines
- Sistema de customización de piloto
- Modos adicionales (Time Attack, etc.)
- Integración de sonido

---

## 12. MÉTRICAS DE ÉXITO

### 12.1 Criterios MVP

Una implementación MVP exitosa si:

```
✓ Jugador puede avanzar automático y saltar
✓ Rival compite con IA básica funcional
✓ Terreno es colisionable y recorrível
✓ Sistema victoria/derrota funciona sin bugs
✓ UI básica muestra resultados correctamente
✓ Narrativa se carga y se muestra
✓ Juego corre a 60 FPS sin stutters
✓ Un nivel es jugable de inicio a fin
✓ Tiempo total de desarrollo ≤ 4 semanas
```

### 12.2 Métricas de Jugabilidad

```
Equilibrio del rival:
  - Ganancia rate si el rival es del mismo nivel: ~50% (50% victoria, 50% derrota)
  - Con buen timing: 70%+ victoria
  - Sin timinnig: 20%+ victoria
```

---

## 13. CAMBIOS & CONTROL DE VERSIÓN

| Versión | Fecha | Cambios |
|---------|-------|---------|
| 1.0 | 24/03/2026 | Documento inicial — MVP specification |
| 1.1 | *Pendiente* | Aprobación y ajustes post-revisión |
| 2.0 | *Futuro* | Expansión a 6 niveles full + Extras |

---

## 14. REFERENCIA RÁPIDA

### Mecánicas Principales (Resumen)
- Avance automático hacia la derecha (5 u/s)
- Salto con Space (5 u/s impulso)
- Rival automático (4+ u/s, salto cada 0.5s)
- Victoria: Llegar primero a meta
- Derrota: Caer o rival gana

### Arquitectura Mínima
```
PlayerController → Movimiento
RivalController → IA
GameManager → Estado
LevelManager → Nivel
UIManager → Interfaz
Terreno/Obstáculos → Prefabs
```

### Duración Proyecto (Estimado)
- MVP: 3-4 semanas
- 6 niveles full: 1-2 semanas adicionales
- Polish: + 1 semana

---

## 15. APÉNDICE: INSPIRACIONES & REFERENCIAS

### Juegos Referencia
- **Excitebike (NES)** — Mecánica de carrera 2D
- **Flappy Bird** — Simplicidad de controles
- **Geometry Dash** — Timing de plataformas
- **Mario Kart** — Concepto de rivalidad

### Tutoriales Unity Recomendados
- Rigidbody2D platformer mechanics
- 2D colliders & raycasting
- Simple state machine
- Canvas UI basics

---

**FIN DEL GDD**

Documento elaborado como especificación viva — sujeto a ajustes iterativos durante desarrollo.
