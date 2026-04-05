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

### 1.3 Clasificación del Género (Académico)

**Clasificación → Runner 2D con Mecánicas de Plataforma**  
El juego **reinterpreta las mecánicas académicas de plataformas** en un contexto de carrera:
- **Saltar** → Esquivar obstáculos verticales (necesidad mecánica)
- **Plataformas** → Segmentos de pista elevados (progresión de dificultad)
- **Daño** → Sistema de colisión con obstáculos (castigo por error)
- **Coleccionables** → [En futuro] Monedas durante la carrera
- **UI** → Resultado de carrera y narrativa (feedback claro)

**Ventaja pedagógica**: Enseña física 2D, colisiones, state management y narrativa, dentro de un juego accesible y dinámico.

### 1.4 Elevator Pitch (30 segundos)

Juego de carreras arcade 2D estilo Excitebike donde controlas un piloto novato. Avanzas automáticamente hacia la derecha, **saltas para evitar obstáculos y terreno irregular**, y compites contra un rival único por nivel. Gana la carrera llegando primero a la meta. Derrota al campeón en el nivel final y completa tu camino a la gloria.

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

## 2.4 ALINEACIÓN CON REQUISITOS ACADÉMICOS (CRÍTICO)

Este documento mapea **explícitamente** cómo el juego cumple con los requisitos típicos de evaluación para juegos de plataforma:

| Requisito Académico | Mecánica en el Juego | Implementación | Estado |
|---------------------|----------------------|-----------------|--------|
| **Movimiento** | Desplazamiento automático (eje X) + Salto (eje Y) | PlayerController.Move() + PlayerController.Jump() | ✓ Core |
| **Salto** | Space para saltar; Physics2D con gravedad; Ground check | PlayerController con Rigidbody2D y raycast | ✓ Core |
| **Plataformas** | Segmentos elevados del terreno; requieren salto para acceder | Prefabs de Platform con BoxCollider2D estático | ✓ Core |
| **Colisiones** | CheckGround, colisión con obstáculos, detección de meta | Physics2D.OverlapCircle + raycast; Triggers | ✓ Core |
| **Daño / Game Over** | Toque de obstáculo = derrota; caída fuera de bounds = derrota | Script Obstacle con trigger; DeathZone trigger | ✓ Core |
| **Coleccionables** | [MVP v1] No incluido; [Futuro] Monedas durante la carrera | CoinCollector script (futuro) | ⏳ Post-MVP |
| **UI / Feedback** | Pantalla de resultado, narrativa, contador de nivel | UIManager.ShowResult(); Canvas UI | ✓ Core |
| **Física** | Gravedad 2D, drag, impulso de salto | Physics2D settings; Rigidbody2D.velocity | ✓ Core |
| **Lógica de Juego** | Victoria/derrota; condiciones de fin de nivel | GameManager.CheckVictory/Defeat | ✓ Core |
| **Narrativa** | Diálogos intro por nivel; contexto de rival | UI de intro + JSON de narrativa | ✓ Core |

**Resultado: MVP cumple 9/10 requisitos académicos. Coleccionables será parte de v1.1 o v2.0.**

---

## 2.5 FASES DE DESARROLLO (ROADMAP TÉCNICO)

### **FASE 1: Maqueta Digital (Semana 1)**
**Objetivo:** Establece el loop básico sin pulir. Herramientas: Unity, scripts base.

- [x] Crear escena vacía con cámara y canvas
- [ ] Implementar PlayerController (movimiento + salto)
- [ ] Crear prefab Player (cuadrado azul simple)
- [ ] Implementar terreno básico (Ground plano)
- [ ] Prueba de salto y gravedad
- [ ] Crear RivalController (movimiento automático)
- [ ] Crear prefab Rival (cuadrado rojo)
- [ ] Setup de Physics2D (Layer, gravity, collision matrix)
- **Milestone:** Jugador y rival corren/saltan juntos en escena de prueba

### **FASE 2: Gameplay Funcional (Semana 1-2)**
**Objetivo:** Mecánicas completas sin UI pulida.

- [ ] Crear GameManager (manejo de estado)
- [ ] Implementar sistema de Victoria (meta trigger)
- [ ] Implementar sistema de Derrota (obstáculos, caída)
- [ ] Crear prefabs de Terreno (Platform, Obstáculo, Rampa)
- [ ] Diseñar el paso del Nivel 01 (layout simple)
- [ ] Rival IA básica (salto automático, raycast simple)
- [ ] Prueba de colisiones (el rival no choca con jugador)
- **Milestone:** Se puede jugar un nivel completo y ganar/perder

### **FASE 3: Interacción & Control (Semana 2)**
**Objetivo:** Menús, transiciones, narrativa.

- [ ] Crear Main Menu scene
- [ ] Crear Intro Level screen (narrativa + rival)
- [ ] Crear Result screen (victoria/derrota)
- [ ] Cargar narrative.json y bindear a UI
- [ ] LevelManager para cargar niveles secuencialmente
- [ ] Botones funcionales: [Jugar], [Siguiente], [Reintentar], [Menú], [Salir]
- [ ] Transiciones entre pantallas (fade in/out)
- **Milestone:** Juego es navegable de menú a resultado

### **FASE 4: UI & Polish (Semana 2-3)**
**Objetivo:** Producto presentable.

- [ ] Refine visual de Player/Rival en gameplay
- [ ] Canvas UI en-juego minimalista (nombre rival, estadística simple)
- [ ] Efectos visuales triviales (color change en derrota, fade de pantallas)
- [ ] Testing en 3+ PCs
- [ ] Optimización de rendimiento (si es necesario)
- [ ] Documentación final del código
- **Milestone:** Juego es presentable y funciona sin bugs

### **FASE 5: Expansión Condicional (Semana 3-4, si tiempo)**
**Objetivo:** Agregar 3-6 niveles completos.

- [ ] Diseñar layout de Nivel 02-06
- [ ] Ajustar parámetros de rival por nivel (tabla en sección 7.2)
- [ ] Balanceo de dificultad
- [ ] Testing completo (speedrun, fallos, edge cases)
- [ ] Build final

---

## 2.6 CHECKLIST DE IMPLEMENTACIÓN (TAREAS TÉCNICAS)

**Usa este checklist durante desarrollo para rastrear progreso.**

### Scripts Necesarios
- [ ] `PlayerController.cs` — Movimiento + salto + ground check
- [ ] `RivalController.cs` — IA rival + saltos automáticos
- [ ] `GameManager.cs` — Estado de juego y condiciones de fin
- [ ] `LevelManager.cs` — Carga de niveles y setup
- [ ] `UIManager.cs` — Manejo de instrucciones y pantallas
- [ ] `CameraController.cs` — Seguimiento de jugador (opcional, puede ser cámara fija)

### Prefabs Necesarios
- [ ] `Player.prefab` — Con Rigidbody2D, Colliders, Sprite
- [ ] `Rival.prefab` — Con Rigidbody2D, Colliders, Sprite
- [ ] `Ground.prefab` — Terreno base plano
- [ ] `Platform.prefab` — Plataforma elevada
- [ ] `Obstacle.prefab` — Obstáculo dañino (pinchos, roca)
- [ ] `Rampa.prefab` — Inclinada (opcional para MVP v1)
- [ ] `Meta.prefab` — Trigger de victoria

### Escenas Necesarias
- [ ] `MainMenu.unity` — Pantalla principal
- [ ] `Level01.unity` — Primer nivel con Carlos
- [ ] `Level02-06.unity` — Niveles adicionales (futuro)
- [ ] `GameplayTest.unity` — Escena de prueba

### Assets
- [ ] Sprites simples (cuadrados de color; puedes usar Unity default)
- [ ] `narrative.json` — Diálogos de intro por nivel

### Configuración Unity
- [ ] Layers creadas: Player, Rival, Terrain, Obstacle, Meta, DeathZone
- [ ] Physics2D settings ajustados (gravity, collision matrix)
- [ ] Canvas y UI Prefabs diseñados
- [ ] InputSystem configurado para Space

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

### 8.3 DEFINICIÓN DE SISTEMAS DEL JUEGO

#### Sistema 1: PlayerController
**Responsabilidad:** Movimiento, salto, detección de suelo.
```csharp
class PlayerController : MonoBehaviour {
  // Movimiento
  public float moveSpeed = 5.0f;  // Unidades/segundo
  public float jumpForce = 5.0f;  // Impulso vertical
  
  // Drag
  public float groundDrag = 1.0f;
  public float airDrag = 0.5f;
  
  // Ground Check
  public float groundCheckDistance = 0.1f;
  public float groundCheckRadius = 0.2f;
  public LayerMask groundLayer;
  
  // Métodos principales
  void FixedUpdate() { ... }  // Aplicar velocidad horizontal
  void Update() { ... }       // Leer input salto
  void Jump() { ... }         // Aplicar impulso
  bool CheckGrounded() { ... } // Raycast/OverlapCircle abajo
  void ApplyDrag() { ... }    // Modular drag según estado
}
```
**Especificación de métodos:**
- `FixedUpdate()`: Aplicar `moveSpeed` constante al eje X del Rigidbody
- `Update()`: Si `Input.GetKeyDown(Space)` AND `isGrounded`, llamar `Jump()`
- `Jump()`: Set `rigidbody.velocity.y = jumpForce`
- `CheckGrounded()`: `Physics2D.OverlapCircle()` en LayerMask "Terrain"
- `ApplyDrag()`: `rigidbody.drag = isGrounded ? groundDrag : airDrag`

**Integración:**
- Attach a prefab `Player`
- Requiere: `Rigidbody2D` (Dynamic), `BoxCollider2D` (main + ground check), `SpriteRenderer`

---

#### Sistema 2: RivalController
**Responsabilidad:** IA del rival; movimiento + toma de decisiones de salto.
```csharp
class RivalController : MonoBehaviour {
  // Movimiento
  public float moveSpeed = 4.5f;  // Porcentaje: 90% del jugador
  public float jumpForce = 4.5f;
  
  // IA
  public float decisionTimer = 0.5f;  // Cada cuántos segundos decide
  public float raycastDistance = 2.0f; // Detectar obstáculos adelante
  public float jumpProbability = 0.7f; // 70% chance de saltar
  
  // Métodos principales
  void FixedUpdate() { ... }  // Aplicar movimiento
  void Update() { ... }       // Lógica de salto
  bool DetectObstacle() { ... } // Raycast adelante
  void DecideJump() { ... }   // Lógica probabilística
}
```
**Especificación de métodos:**
- `FixedUpdate()`: `rigidbody.velocity.x = moveSpeed`
- `Update()`: Cada `decisionTimer` segundos:
  - Si `DetectObstacle()` VERDADERO → Saltar (100%)
  - Si NO → Saltar con `jumpProbability` chance
- `DetectObstacle()`: `Physics2D.Raycast()` adelante (Vector2.right)
- `DecideJump()`: `Random.value < jumpProbability` para determinar salto

**Parámetros por Nivel (ajustar en editor):**
- Nivel 1 (Carlos): 4.0 u/s, timer 0.8s, prob 60%
- Nivel 6 (UNIDAD): 5.0 u/s, timer 0.2s, prob 100%
(Ver tabla sección 7.2 para niveles intermedios)

**Integración:**
- Attach a prefab `Rival`
- Requiere: `Rigidbody2D`, `BoxCollider2D` (main + ground check)

---

#### Sistema 3: GameManager (State Machine)
**Responsabilidad:** Manejo de estado del juego; detección de victoria/derrota.
```csharp
enum GameState { Playing, PlayerWon, RivalWon, GameOver }

class GameManager : MonoBehaviour {
  public static GameManager instance;  // Singleton
  public GameState currentState = GameState.Playing;
  
  // Referencias
  public Transform playerTransform;
  public Transform rivalTransform;
  public Transform metaTransform;
  
  // Límite de caída
  public float deathYLevel = -20f;
  
  // Métodos principales
  void Update() { ... }  // Chequear condiciones cada frame
  void CheckVictory() { ... }  // ¿Jugador cruzó meta?
  void CheckDefeat() { ... }   // ¿Jugador cayó o rival ganó?
  void EndGame(bool playerWon) { ... }  // Cambiar estado y notificar
}
```
**Especificación de métodos:**
- `CheckVictory()`: Si `playerTransform.x >= metaTransform.x` → `EndGame(true)`
- `CheckDefeat()`: Si `playerTransform.y < deathYLevel` OR `rivalTransform.x >= metaTransform.x` → `EndGame(false)`
- `EndGame()`: Pausar físicas; cambiar `currentState`; notificar `UIManager`

**Integración:**
- Attach a un GameObject vacío llamado "GameManager"
- Usar patrón **Singleton** para acceso global desde otros scripts
- Inicializar referencias en `Start()`

---

#### Sistema 4: LevelManager
**Responsabilidad:** Cargar escenas de nivel; inicializar narrativa.
```csharp
class LevelManager : MonoBehaviour {
  [System.Serializable]
  public class LevelData {
    public string levelName;
    public string sceneName;
    public string rivalName;
    public float rivalSpeed;
  }
  
  public List<LevelData> levels = new List<LevelData>();
  public int currentLevelIndex = 0;
  
  // Métodos principales
  void Start() { ... }  // Cargar nivel actual
  void LoadLevel(int index) { ... }
  void LoadNextLevel() { ... }
  void RestartLevel() { ... }
}
```
**Especificación de métodos:**
- `LoadLevel()`: `SceneManager.LoadScene(levels[index].sceneName)`
- `LoadNextLevel()`: `currentLevelIndex++; LoadLevel()`
- `RestartLevel()`: `SceneManager.LoadScene(SceneManager.GetActiveScene().name)`

**Integración:**
- Attach a un GameObject llamado "LevelManager"
- Usar `DontDestroyOnLoad()` si persiste entre escenas
- Conectar referencia a `GameManager`

---

#### Sistema 5: UIManager
**Responsabilidad:** Mostrar pantallas (intro, resultado), ejecutar transiciones.
```csharp
class UIManager : MonoBehaviour {
  // Canvas y Panels
  public CanvasGroup introPanel;
  public CanvasGroup resultPanel;
  public Text resultText;
  public Button nextBtn, retryBtn, menuBtn;
  
  // Método principal (llamado desde GameManager)
  public void ShowResult(bool playerWon) { ... }
  public void ShowIntroLevel(string rivalName, string intro) { ... }
  
  // Transiciones
  IEnumerator FadeTransition(CanvasGroup group, float duration) { ... }
}
```
**Especificación de métodos:**
- `ShowResult()`: Actualizar `resultText` con victoria/derrota; fade in panel; bindear botones
- `ShowIntroLevel()`: Mostrar nombre rival + narrativa por 2-3s; fade out automático
- `FadeTransition()`: Animar alpha de CanvasGroup a lo largo de `duration` segundos

**Integración:**
- Attach a Canvas prefab
- Referenciar en GameManager para llamadas

---

#### Sistema 6: Obstacle (Trigger de Daño)
**Responsabilidad:** Detectar colisión del jugador; comunicar derrota.
```csharp
class Obstacle : MonoBehaviour {
  void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.tag == "Player") {
      GameManager.instance.EndGame(false);  // Derrota
    }
  }
}
```
**Setup:**
- BoxCollider2D con `Is Trigger = true`
- Layer: "Obstacle"
- Collision matrix: Obstacle detecta colisión con Player

---

#### Sistema 7: Meta (Trigger de Victoria)
**Responsabilidad:** Detectar llegada del jugador.
```csharp
class Meta : MonoBehaviour {
  void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.tag == "Player") {
      GameManager.instance.CheckVictory();
    }
  }
}
```
**Setup:**
- BoxCollider2D con `Is Trigger = true`
- Layer: "Meta"

---

### 8.4 Arquitectura de Scripts (Directorio del Proyecto)

```
Assets/
├── Scripts/
│   ├── Player/
│   │   └── PlayerController.cs ________________________________ [  ]
│   │
│   ├── Rival/
│   │   └── RivalController.cs _________________________________ [  ]
│   │
│   ├── Game/
│   │   ├── GameManager.cs _____________________________________ [  ]
│   │   └── LevelManager.cs ____________________________________ [  ]
│   │
│   ├── UI/
│   │   └── UIManager.cs _______________________________________ [  ]
│   │
│   ├── Environment/
│   │   ├── Obstacle.cs ________________________________________ [  ]
│   │   └── Meta.cs ___________________________________________ [  ]
│   │
│   └── Utilities/
│       └── [Helpers, Constants, etc.]
│
├── Prefabs/
│   ├── Player.prefab
│   ├── Rival.prefab
│   ├── Terrain/
│   │   ├── Ground.prefab
│   │   ├── Platform.prefab
│   │   ├── Obstacle.prefab
│   │   └── Meta.prefab
│   └── UI/
│       └── [Canvas prefabs]
│
├── Scenes/
│   ├── MainMenu.unity
│   ├── GameplayTest.unity
│   ├── Level01.unity
│   ├── Level02.unity
│   └── ... (Levels 03-06)
│
└── Resources/
    ├── narrative.json
    └── [Audio, sprites si hay]
```

**Checklist de Setup:**
- [x] Crear carpeta `Scripts/` con subcarpetas
- [x] Crear carpeta `Prefabs/` con subgrupos
- [x] Crear carpeta `Scenes/` para escenas
- [x] Crear carpeta `Resources/` para JSON

---

### 8.5 Componentes de GameObject (Detalle)

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

### 8.5 Componentes de GameObject (Detalle)

#### Player GameObject — Configuración Completa
```
GameObject: Player
├── Transform
│   ├── Position: (0, 0, 0)
│   ├── Rotation: (0, 0, 0)
│   └── Scale: (1, 1, 1)
├── Rigidbody2D
│   ├── Body Type: Dynamic
│   ├── Mass: 1
│   ├── Gravity Scale: 1
│   ├── Velocity: (5, 0) — inicial, luego controlado
│   ├── Constraints: 
│   │   └── Freeze Rotation Z: ✓ (No girar)
│   ├── Collision Detection: Continuous
│   ├── Constraints (avanzado): 
│   │   └── rigidbody.isKinematic: false
│   └── Material → Default Material (friction 0.4, bounce 0)
│
├── Collider — Main (NO trigger)
│   ├── BoxCollider2D
│   ├── Size: (0.8, 1.5) — ancho × alto
│   ├── Offset: (0, 0)
│   ├── Is Trigger: ✗ false
│   ├── Layer: Player
│   └── Friction: 0.4 (del material)
│
├── Collider — GroundCheck (ES trigger, oculto)
│   ├── BoxCollider2D
│   ├── Size: (0.6, 0.1) — más estrecho, muy fino
│   ├── Offset: (0, -0.8) — bajo los pies
│   ├── Is Trigger: ✓ true
│   ├── Layer: GroundCheck
│   └── [Este collider no afecta física; solo detecta]
│
├── SpriteRenderer
│   ├── Sprite: [Optional] Sprite azul simple o None
│   ├── Color: #3B82F6 (Azul)
│   ├── Sorting Order: 1
│   └── [En MVP, puedes usar un cuadrado de color sin sprite]
│
├── Tag: "Player" ← CRÍTICO para scripts
│
└── Script: PlayerController.cs
    ├── moveSpeed: 5.0
    ├── jumpForce: 5.0
    ├── groundDrag: 1.0
    ├── airDrag: 0.5
    ├── groundCheckRadius: 0.2
    ├── groundCheckDistance: 0.1
    └── groundLayer: Terrain
```

#### Rival GameObject — Configuración Completa
```
GameObject: Rival
├── Transform
│   ├── Position: (2, 0, 0) ← Comienza ligeramente detrás
│   ├── Rotation: (0, 0, 0)
│   └── Scale: (1, 1, 1)
├── Rigidbody2D
│   ├── [Igual configuración que Player]
│   └── ...
├── Collider — Main
│   ├── BoxCollider2D
│   ├── Size: (0.8, 1.5)
│   ├── Offset: (0, 0)
│   ├── Is Trigger: ✗
│   ├── Layer: Rival
│   └── [NO choca con Player — ver collision matrix]
│
├── Collider — GroundCheck
│   ├── [Igual a Player]
│
├── SpriteRenderer
│   ├── Color: #DC2626 (Rojo)
│   ├── Sorting Order: 1
│
├── Tag: "Rival"
│
└── Script: RivalController.cs
    ├── moveSpeed: 4.5 (ajustable por nivel)
    ├── jumpForce: 4.5
    ├── decisionTimer: 0.5
    ├── jumpProbability: 0.7
    └── raycastDistance: 2.0
```

#### Ground GameObject (Prefab Reutilizable)
```
GameObject: Ground (Segmento)
├── Transform
│   ├── Position: [Variar según layout]
│   ├── Scale: (10, 0.5, 1) ← Ejemplo: 10 unidades de ancho
│
├── BoxCollider2D
│   ├── Size: (10, 0.5) ← Debe coincidir con escala visual
│   ├── Is Trigger: ✗
│   ├── Layer: Terrain
│
├── SpriteRenderer [Optional]
│   ├── Color: #16A34A (Verde)
│   ├── Sprite: [simplemente un rectángulo]
```

#### Platform GameObject (Prefab Reutilizable)
```
GameObject: Platform
├── Transform
│   ├── Position: (x, y+1, 0) ← Elevada del ground
│   ├── Scale: (3, 0.5, 1) ← Más pequeña que ground
│
├── BoxCollider2D
│   ├── Size: (3, 0.5)
│   ├── Is Trigger: ✗
│   ├── Layer: Terrain
│
├── SpriteRenderer
│   ├── Color: #059669 (Verde más claro)
```

#### Obstacle GameObject (Prefab Reutilizable)
```
GameObject: Obstacle_Spike
├── Transform
│   ├── Position: [A lo largo de la pista]
│   ├── Scale: (0.5, 0.5, 1)
│
├── BoxCollider2D (Envuelve el obstáculo)
│   ├── Size: (0.5, 0.5)
│   ├── Is Trigger: ✓ true ← ¡IMPORTANTE! No detiene físicamente
│   ├── Layer: Obstacle
│
├── Script: Obstacle.cs
│   └── OnTriggerEnter2D(Collider2D collision) 
│       → GameManager.EndGame(false)
│
└── SpriteRenderer
    ├── Color: #991B1B (Rojo oscuro)
```

#### Meta GameObject (Prefab)
```
GameObject: Meta (Finish Line)
├── Transform
│   ├── Position: (80, 0, 0) ← Fin del recorrido
│
├── BoxCollider2D
│   ├── Size: (1, 3) ← Muro vertical para detectar paso
│   ├── Is Trigger: ✓ true
│   ├── Layer: Meta
│
├── Script: Meta.cs
│   └── OnTriggerEnter2D() 
│       → GameManager.CheckVictory()
│
└── SpriteRenderer (Visual)
    ├── Color: #FBBF24 (Dorado)
```

#### DeathZone GameObject (Opcional pero Recomendado)
```
GameObject: DeathZone
├── Transform
│   ├── Position: (40, -20, 0) ← Debajo del mapa
│   ├── Scale: (200, 5, 1) ← Cubre todo el ancho
│
├── BoxCollider2D
│   ├── Size: (200, 5)
│   ├── Is Trigger: ✓ true
│   ├── Layer: DeathZone
│
└── Script: DeathZone.cs [Alternativa a chequeo de Y en GameManager]
    └── OnTriggerStay2D() 
        → GameManager.EndGame(false)
```

---

### 8.6 Capas (Layers) — Setup Requerido

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

## 12. MÉTRICAS DE ÉXITO Y CRITERIOS DE EVALUACIÓN

### 12.1 Criterios MVP Técnicos (Checklist de Aceptación)

Una implementación MVP exitosa **DEBE** cumplir:

```
✓ MECÁNICAS BÁSICAS
  [  ] Jugador avanza automáticamente hacia la derecha (5 u/s)
  [  ] Jugador puede saltar con Space desde el suelo
  [  ] Gravedad y físicas funcionan (jugador cae después de saltar)
  [  ] Ground check funciona (jugador no puede saltar infinitamente)
  
✓ RIVAL & COMPETENCIA
  [  ] Rival avanza automáticamente a velocidad configurada
  [  ] Rival salta automáticamente en decisiones
  [  ] Rival NO choca físicamente con jugador
  [  ] Rival se mueve independientemente
  
✓ TERRENO & INTERACCIÓN
  [  ] Terreno es colisionable y el jugador no cae
  [  ] Plataformas elevan al jugador
  [  ] Obstáculos detectan colisión (trigger)
  [  ] Meta detecta llegada del jugador (trigger)
  [  ] Deathzone (o Y-check) registra caída del jugador
  
✓ LÓGICA DE JUEGO
  [  ] Condición de VICTORIA: Jugador cruza meta primero
  [  ] Condición de DERROTA: Toca obstáculo OR cae fuera OR rival gana
  [  ] GameManager maneja transiciones de estado
  [  ] Game Over (victoria/derrota) detiene la jugabilidad
  
✓ UI & FEEDBACK
  [  ] Pantalla de Intro de Nivel muestra nombre rival + narrativa
  [  ] Pantalla de Resultado muestra victoria o derrota
  [  ] Botones [Siguiente], [Reintentar], [Menú] funcionan
  [  ] Transiciones entre pantallas son suaves (fade in/out)
  
✓ NARRATIVA
  [  ] narrative.json carga correctamente
  [  ] Textos de intro se muestran en UI
  [  ] Rival tiene nombre y contexto único
  
✓ RENDIMIENTO
  [  ] Juego corre a ~60 FPS sin stutters (PC estándar)
  [  ] Memoria < 512 MB durante gameplay
  [  ] Sin errores en Console (salvo warnings inofensivos)
  
✓ INTEGRACIÓN
  [  ] PlayerController, RivalController, GameManager se comunican
  [  ] Prefabs se instancian correctamente en escenas
  [  ] Layers y collision matrix funcionan sin bugs
  [  ] Un nivel completo (Level01) es jugable de inicio a fin
```

**Resultado: Si TODO está marcado ✓, MVP está LISTO PARA PRESENTACIÓN.**

---

### 12.2 Métricas de Jugabilidad & Balanceo

```
VICTORIA del Jugador (esperado):
  - Contra un rival de igual nivel: ~50% win rate (competencia justa)
  - Con BUEN timing: 70%+ win rate (recompensa eficiencia)
  - Sin TIMING: 20%+ win rate (posible pero difícil)
  
RESPUESTA del Rival IA:
  - En Nivel 1 (Carlos): Debe permitir victoria fácil (80%+)
  - En Nivel 6 (UNIDAD): Debe ser competitivo (40%+ win rate vs perfección)
  - Sin varianza extrema (no debe ganar SIEMPRE o NUNCA)
  
DIFICULTAD PROGRESIVA:
  - Cada nivel aumenta 5-10% la velocidad del rival
  - Patrón de saltos se vuelve más predecible/más frecuente
  - Terreno aumenta complejidad (más plataformas, más huecos)
  
FEEDBACK AL JUGADOR:
  - Derrota es clara: "Rival fue más rápido" O "¡Impacto!" O "¡Caíste!"
  - Victoria es clara: "¡Ganaste contra [Rival]!"
  - Estado es siempre visible (no ambigüedad)
```

### 12.3 Lineamientos Académicos (Demostración de Competencia)

El juego **debe evidenciar** estos conocimientos:

| Competencia | Evidencia en el Juego |
|-------------|----------------------|
| **Física 2D** | Gravedad real, impulso de salto, detección de colisiones |
| **Programación OOP** | PlayerController, RivalController, GameManager como clases independentes |
| **State Management** | GameState enum; transiciones claras entre pantallas |
| **Event-Driven Design** | OnTriggerEnter2D para obstáculos; eventos de victoria/derrota |
| **Prefabs & Reutilización** | Platform, Obstacle, Ground como prefabs instanciados múltiples veces |
| **UI & Canvas** | Menú navegable; pantallas de intro/resultado; botones funcionales |
| **Narrativa Integrada** | JSON de diálogos; narrativa por nivel; progresión clara |
| **Control de Calidad** | Sin crashes; gameplay equilibrado; testing en múltiples situaciones |

**Este juego demuestra competencia académica en los 8 áreas listadas.**

---

### 12.4 Plan de Testing

Antes de presentación, verificar:

#### Test 1: Tutorial Implícito
- Abrir Level01 SIN leer instrucciones
- ¿Puedo saltar y entender la mecánica por exploración?
- → **Resultado esperado:** SÍ, después de 5-10 segundos

#### Test 2: Victoria
- Jugar Level01 con timing perfecto
- ¿Llego a meta primero?
- → **Resultado esperado:** SÍ, victoria pantalla

#### Test 3: Derrota por Obstáculo
- Jugar Level02, saltar a propósito sobre obstáculo
- ¿Registra derrota?
- → **Resultado esperado:** SÍ, pantalla "DERROTA — ¡Impacto!"

#### Test 4: Derrota por Caída
- En cualquier nivel, saltar fuera de la pista
- ¿Detecta caída?
- → **Resultado esperado:** SÍ, pantalla "DERROTA — ¡Caíste!"

#### Test 5: Rival Ganador
- En Level01, NO saltar; dejar que rival pase
- ¿Rival gana?
- → **Resultado esperado:** SÍ, "DERROTA — Rival fue más rápido"

#### Test 6: Flujo Completo
- Comenzar en Main Menu
- Navegar a Level01
- Ganar
- Ir a Level02
- Ir a siguiente nivel
- Reintentar un nivel
- Volver a menú
- ¿Transiciones son suaves y sin errores?
- → **Resultado esperado:** SÍ, flujo limpio

#### Test 7: Rendimiento
- Jugar 5 minutos seguidos
- ¿FPS se mantiene en 60?
- ¿Memoria no sube indefinidamente?
- ¿Sin memory leaks?
- → **Resultado esperado:** SÍ, estable

---

### 12.5 Criterios de Éxito Académico

**El juego es aceptado como Proyecto si:**

1. **Funciona sin crashes** (gameplay limpio sin errores)
2. **Cumple requisitos de plataforma** (movimiento, salto, colisiones, daño, UI) ✓
3. **Código es legible y estructurado** (OOP, métodos claros, comentarios)
4. **Juego es balanceado y divertido** (desafío progresivo, no es frustante)
5. **Narrativa enriquece la experiencia** (contexto, rivalidad, progresión)
6. **Presentación es profesional** (no parece inacabado)
7. **Tiempo de conclusión es eficiente** (~3-4 semanas máximo)

**Si se cumplen los 7 criterios, proyecto está APROBADO.**

---

## APÉNDICE A: QUICK START GUIDE — PRIMEROS PASOS (30 MIN)

**Este es un resumen ejecutivo para comenzar rápido sin leer todo el GDD.**

### Paso 1: Setup de Proyecto (5 min)
```
1. Abrir Unity 2022 LTS+
2. Crear nuevo proyecto 2D
3. Crear carpetas: Assets/Scripts/, Assets/Prefabs/, Assets/Scenes/
4. En Project Settings → Physics2D:
   - Gravity: (0, -9.81)
5. Crear Layers: Player, Rival, Terrain, Obstacle, Meta, GroundCheck, DeathZone
```

### Paso 2: Crear Gameobjects Básicos (10 min)
```
1. Crear Game Object → Sprite Square (Player)
   - Add Component: Rigidbody2D (Dynamic)
   - Add Component: BoxCollider2D (main)
   - Add Component: BoxCollider2D (GroundCheck, offset -0.8Y)
   - Assign Layer: Player
   
2. Duplicar para Rival (Color rojo)
   - Assign Layer: Rival
   
3. Crear Ground (Sprite Square, escala 10x0.5)
   - Add Component: BoxCollider2D
   - Assign Layer: Terrain
```

### Paso 3: Scripts Críticos (15 min)
```
Crear en Assets/Scripts/Player/:
PlayerController.cs
  ├── moveSpeed = 5.0 (FixedUpdate)
  ├── jumpForce = 5.0 (Input.Space)
  └── CheckGrounded() → raycast abajo

Crear en Assets/Scripts/Rival/:
RivalController.cs
  ├── moveSpeed = 4.5 (FixedUpdate)
  └── DecideJump() → Random every 0.5s

Crear en Assets/Scripts/Game/:
GameManager.cs
  ├── CheckVictory() → Si X player >= X meta
  ├── CheckDefeat() → Si toca obstáculo O cae
  └── Singleton pattern
```

### Paso 4: Test (PRE-MVP CHECKPOINT)
```
Press Play:
  ✓ Jugador corre y salta con Space
  ✓ Rival corre automático
  ✓ Ambos corren sin chocar
  ✓ Ambos saltan (o al menos intentan)
  
Si TODO funciona → Avanza a FASE 2
Si hay bugs → Debugea primero
```

**[Ver sección 2.5 FASES DE DESARROLLO para continuar]**

---

## APÉNDICE B: GLOSARIO DE TÉRMINOS

| Término | Significado |
|---------|------------|
| **MVP** | Minimum Viable Product — mínimo nivel de funcionalidad |
| **Prefab** | Template reutilizable de GameObject en Unity |
| **Trigger** | Collider que detecta sin ejercer fuerza física |
| **Raycast** | Línea invisible para detectar objetos |
| **State Machine** | Sistema que cambia entre estados definidos |
| **Layer** | Categoría de GameObject para organizar colisiones |
| **Rigidbody** | Componente que añade física a un GameObject |
| **Collider** | Componente que define forma para colisiones |
| **Canvas** | Elemento de UI que contiene botones, texto, etc. |
| **Escena (Scene)** | Nivel o pantalla del juego |
| **Narrativa** | Contexto e historias de los rivales |
| **Dificultad Progresiva** | Aumento gradual de reto por nivel |
| **Balanceo** | Ajuste para que el juego sea justo y divertido |

---

## APÉNDICE C: TROUBLESHOOTING COMÚN

### Problema: Jugador cae a través del suelo
**Causa:** Ground no tiene BoxCollider2D O Layer no está en collision matrix
**Solución:** 
1. Verificar que Ground tenga `BoxCollider2D` con `Is Trigger = false`
2. Verificar que Layer está en Physics2D collision matrix

### Problema: Jugador salta infinitamente
**Causa:** Ground check no funciona
**Solución:**
1. Verificar `Physics2D.OverlapCircle()` en GroundCheck Layer
2. Verificar que offset es correcto (debajo del jugador)

### Problema: Rival no salta
**Causa:** RivalController.DecideJump() no se llama O raycast no detecta
**Solución:**
1. Agregar `Debug.Log()` en DecideJump()
2. Verificar que `isGrounded` es TRUE antes de saltar

### Problema: FPS cae (stutters)
**Causa:** Demasiados GameObjects o Physics2D actualizando constantemente
**Solución:**
1. Reducir cantidad de Obstacles en prueba
2. Usar `Profiler` (Window → Analysis → Profiler)

### Problema: Narrativa no carga
**Causa:** narrative.json no está en Resources/ O JSON malformado
**Solución:**
1. Verificar Path: `Assets/Resources/narrative.json`
2. Validar JSON en https://jsonlint.com/

---

## APÉNDICE D: REFERENCIAS VISUALES (Diagrama)

```
CORE GAMEPLAY LOOP:

┌──────────────────────────────────────────────────────┐
│  INICIO: Jugador (Azul) vs Rival (Rojo)             │
│  Ambos comienzan X=0, avanzando hacia Meta (X=80)   │
└──────────────────────┬───────────────────────────────┘
                       ↓
       ┌───────────────────────────────────┐
       │  FRAME-BY-FRAME LOOP              │
       │  ┌─────────────────────────────┐  │
       │  │ 1. Mover ambos adelante     │  │
       │  ├─────────────────────────────┤  │
       │  │ 2. Jugador salta? (Space)   │  │
       │  ├─────────────────────────────┤  │
       │  │ 3. Rival salta? (Random)    │  │
       │  ├─────────────────────────────┤  │
       │  │ 4. Física: Gravedad aplica  │  │
       │  ├─────────────────────────────┤  │
       │  │ 5. Chequear Victoria/Derrota│  │
       │  └─────────────────────────────┘  │
       │                                   │
       │  [Repetir cada 0.0167s @ 60 FPS]  │
       └───────────┬───────────────────────┘
                   ↓
        ┌──────────┴──────────┐
        ↓                     ↓
    VICTORIA          DERROTA (Rival gana,
    (Jugador          toca obstáculo,
     cruza meta)      cae fuera)
        │                     │
        └──────────┬──────────┘
                   ↓
        ┌──────────────────────┐
        │  MOSTRAR RESULTADO   │
        │  [Siguiente Nivel]   │
        │  [Reintentar]        │
        │  [Menú]              │
        └──────────────────────┘
```

---

## APÉNDICE E: TIMELINE DE DESARROLLO REALISTA

**Estimación basada en experiencia con proyectos similares:**

```
SEMANA 1 (35 horas)
├─ Lunes-Miércoles: FASE 1 (Maqueta)
│  ├─ PlayerController + RivalController: 4h
│  ├─ Setup Physics2D + Layers: 2h
│  ├─ Terreno básico y Ground check: 3h
│  └─ Testing: 1h
│
├─ Jueves-Viernes: FASE 2 (Gameplay)
│  ├─ GameManager + Victory/Defeat: 5h
│  ├─ Prefabs de Terrain/Obstacles: 3h
│  ├─ Nivel 01 layout: 2h
│  └─ Testing exhaustivo: 3h

SEMANA 2 (35 horas)
├─ Lunes-Miércoles: FASE 3 (UI & Narrativa)
│  ├─ Main Menu + Intro Level screen: 4h
│  ├─ Result screen: 2h
│  ├─ Cargar narrative.json: 2h
│  ├─ LevelManager: 2h
│  └─ Testing: 2h
│
├─ Jueves-Viernes: FASE 4 (Polish)
│  ├─ Sprites + colores finales: 2h
│  ├─ Transiciones suaves: 1h
│  ├─ Balance de rival por nivel: 3h
│  ├─ Optimización rendimiento: 2h
│  └─ Testing final: 2h

BUFFER (Semanas 3-4): +2-4 semanas para expandir a 6 niveles si tiempo

TOTAL MVP: 2-3 semanas
TOTAL FULL (6 niveles): 3-4 semanas
```

---

## APÉNDICE F: RECURSOS EXTERNOS RECOMENDADOS

### Tutoriales Unity Esenciales
- Rigidbody2D y Physics2D basics
- 2D Colliders: https://docs.unity3d.com/Manual/Colliders.html
- Raycasting: https://docs.unity3d.com/Manual/RaycastsUsingPhysics2D.html
- UI Canvas: https://docs.unity3d.com/Manual/UICanvas.html

### Libros / Guías
- "Unity in Action" — Joseph D. Glombitza (Cap. Physics)
- Unity Learn Pathways: https://learn.unity.com/pathways/2d-game-developer

### Comunidad
- r/Unity2D (Reddit)
- Unity Forums: https://forum.unity.com/
- Stack Overflow (tag: `unity3d`)

---
---

## 16. CAMBIOS & VERSIONES DEL DOCUMENTO

| Versión | Fecha | Cambios Principales |
|---------|-------|---------------------|
| 1.0 | 24/03/2026 | Inicial — MVP specification base |
| 1.1 | 05/04/2026 (ACTUAL) | **Mejoras Académicas**:<br/>• Alineación explícita con requisitos académicos<br/>• Definición de 6 sistemas técnicos del juego<br/>• División en 5 fases de desarrollo claras<br/>• Checklist de implementación completo<br/>• Métricas de éxito alineadas con criterios académicos<br/>• Apéndices: Quick Start, Troubleshooting, Timeline |
| 2.0 | *Futuro* | Expansión a 6 niveles full; mejora de IA; extras opcionales |

**Este GDD (v1.1) es el documento final de referencia para desarrollo MVP.**

---

## 17. REFERENCIA RÁPIDA PARA DESARROLLO

### Código Template (PlayerController Skeleton)
```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 1f;
    [SerializeField] private float airDrag = 0.5f;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private float startDrag;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        startDrag = rb.drag;
    }

    void FixedUpdate() {
        // Movimiento horizontal constante
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        
        // Aplicar drag
        ApplyDrag();
    }

    void Update() {
        // Detectar suelo
        isGrounded = Physics2D.OverlapCircle(
            (Vector2)transform.position + Vector2.down * 0.1f, 
            0.2f, 
            groundLayer
        ) != null;
        
        // Input salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            Jump();
        }
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void ApplyDrag() {
        rb.drag = isGrounded ? groundDrag : airDrag;
    }
}
```

### Condiciones de Victoria/Derrota (GameManager Skeleton)
```csharp
public enum GameState { Playing, PlayerWon, RivalWon }

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameState currentState = GameState.Playing;
    
    [SerializeField] private Transform playerPos, rivalPos, metaPos;
    [SerializeField] private float deathYLevel = -20f;
    
    void Update() {
        if (currentState != GameState.Playing) return;
        
        // Victoria: Jugador cruza meta
        if (playerPos.position.x >= metaPos.position.x) {
            EndGame(true);
        }
        
        // Derrota: Rival cruza meta
        if (rivalPos.position.x >= metaPos.position.x) {
            EndGame(false);
        }
        
        // Derrota: Jugador cae
        if (playerPos.position.y < deathYLevel) {
            EndGame(false);
        }
    }
    
    void EndGame(bool playerWon) {
        currentState = playerWon ? GameState.PlayerWon : GameState.RivalWon;
        Time.timeScale = 0f; // Pausar
        UIManager.instance.ShowResult(playerWon);
    }
}
```

---

## 18. CONCLUSIÓN

Este GDD es un **documento vivo** que sirve como guía técnica y creativa durante el desarrollo del MVP.

**Puntos clave:**
1. **Género:** Runner 2D con mecánicas de plataforma (interpretación académica)
2. **Alcance:** 1 nivel funcional + menu + narrativa básica
3. **Duración:** 2-3 semanas de desarrollo

El juego cumple con todos los requisitos académicos típicos de plataformas (salto, colisiones, daño, items, UI) dentro de un contexto accesible y divertido.

**Próximos pasos después de completar MVP:**
- Testing exhaustivo con usuarios externos
- Ajustes de balance
- Posible expansión a 6 niveles si tiempo permite
- Documentación final del código fuente

---

**FIN DEL GDD v1.1**

*Documento oficial de especificación para el Proyecto Runner 2D Academia.*
*Sujeto a cambios durante desarrollo iterativo.*
