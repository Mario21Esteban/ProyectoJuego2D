# TECHNICAL_NOTES — Decisiones Arquitectónicas

**Registro de decisiones técnicas, trade-offs y soluciones implementadas.**

---

## Decisiones de Diseño de Alto Nivel

### 1. Arquitectura: Singleton vs Dependency Injection

**Decisión**: Usar **Singleton para GameManager, LevelManager, UIManager**

**Justificación**:
- Alcance del proyecto: Pequeño MVP académico (~2-3 semanas)
- Simplicidad: Singleton es más fácil learn & debug para principiantes
- Evita overhead de DI framework (Zenject no necesario aún)
- Acceso global desde scripts sin pasar referencias = menos código boilerplate

**Trade-off**:
- ❌ Menos testeable unitarily
- ❌ Difícil de escalar si el proyecto crece
- ✅ Faster prototyping
- ✅ Código más legible

**Revisión potencial**: Si en futuro v2.0 escalamos, considerar DI.

---

### 2. Física: Rigidbody2D Dinámico vs Cinemático

**Decisión**: **Rigidbody2D Dynamic** con movimiento controlado en `FixedUpdate()`

**Justificación**:
- Necesitamos gravedad real para saltos (no podemos simular con animation)
- Colisiones con triggers deben funcionar (OverlapCircle es más robusto en Dynamic)
- Physics2D es nativa en Unity 2D

**Implementación**:
```csharp
// FixedUpdate: aplicar velocidad horizontal
rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

// Update: leer input salto
if(Input.GetKeyDown(Space) && isGrounded) Jump();

// Jump: modifica solo Y
rb.velocity = new Vector2(rb.velocity.x, jumpForce);
```

**Trade-off**:
- ✅ Física consistente 60 FPS
- ✅ Colisiones precisas
- ❌ Debe cuidarse el orden FixedUpdate/Update (se documenta)

---

### 3. Ground Check: OverlapCircle vs Raycast

**Decisión**: **Physics2D.OverlapCircle()** con LayerMask GroundCheck

**Justificación**:
- Más robusto: detecta múltiples colliders en zona
- Más fácil de tunear (radio es intuitivo)
- Menos propenso a falsos negativos en bordes
- Raycast individual puede "atravesar" bordes en velocidades altas

**Implementación**:
```csharp
isGrounded = Physics2D.OverlapCircle(
    (Vector2)transform.position + Vector2.down * groundCheckDistance,
    groundCheckRadius,
    groundLayer
) != null;
```

**Parámetros**:
- `groundCheckDistance`: 0.1 unidades bajo los pies
- `groundCheckRadius`: 0.2 unidades
- `groundLayer`: LayerMask "Terrain"

**Trade-off**:
- ✅ Muy confiable
- ✅ Fácil de visualizar (Debug.DrawCircle)
- ❌ Levemente más costoso que raycast (negligible en MVP)

---

### 4. Salto del Rival: Raycast Adelante vs Estado Machine

**Decisión**: **Raycast simple + Random probability** (sin state machine)

**Justificación**:
- MVP debe ser simple: evitar complexity innecesaria
- Raycast advierte de obstáculos 2 unidades adelante
- Si detecta algo → saltar 100%
- Si NO → saltar con 70% de probabilidad (evita patrón predecible)

**Pseudocódigo**:
```csharp
if (Time.time - lastDecisionTime >= decisionTimer) {
    bool obstacle = Physics2D.Raycast(
        transform.position, Vector2.right, raycastDistance, LayerMask.GetMask("Terrain")
    ).collider != null;
    
    if (obstacle || Random.value < jumpProbability) {
        Jump();
    }
    lastDecisionTime = Time.time;
}
```

**Trade-off**:
- ✅ Simple & funcional
- ✅ Suficiente para MVP
- ❌ Predecible si jugador observa bien
- ❌ No es "inteligente" en todos los escenarios

**Mejora futura**: State machine con "persecución" si rival está muy atrás.

---

### 5. Colisiones: Trigger vs Collision Física

**Decisión**: **Triggers para Obstacle, Meta, DeathZone; Collision normal para terreno**

**Justificación**:
- **Obstacles**: Trigger porque NO queremos bounce física; queremos detección limpia
- **Meta**: Trigger porque solo marcamos pass; no necesita física
- **Terreno**: Collision normal porque el player debe apoyarse

**Setup en Collision Matrix**:
```
Player    ↔ Terrain   = ✓ (físico)
Player    ↔ Obstacle  = ✓ (trigger)
Player    ↔ Meta      = ✓ (trigger)
Rival     ↔ Terrain   = ✓ (físico)
Rival     ↔ Obstacle  = ✓ (trigger, opcional para simetría)
Player    ↔ Rival     = ✗ (sin colisión: pueden ocupar mismo espacio)
```

**Trade-off**:
- ✅ Separación clara: física vs detección
- ✅ Fácil de tunear
- ❌ Si Player y Rival colisionan visualmente (sin física), puede confundir

---

### 6. Narrativa: JSON Hardcoding vs Scriptable Objects

**Decisión**: **JSON simple** en `Assets/Resources/narrative.json`

**Justificación**:
- MVP requiere solo 6 niveles (texto simple)
- JSON es standard, fácil de editar sin ingresa a Unity
- `Resources.Load` es simple para MVP
- Evita crear scriptable object boilerplate

**Estructura JSON**:
```json
{
  "levels": [
    {
      "level_id": 1,
      "rival_name": "Carlos",
      "intro": "Tu mejor amigo Carlo decide competir contigo.",
      "victory": "¡Ganaste! Carlos te anima a continuar.",
      "defeat": "Carlos fue más rápido esta vez."
    }
    // ... más niveles
  ]
}
```

**Trade-off**:
- ✅ Fácil de mantener
- ✅ No requiere editor UI extra
- ❌ Sin validación dentro de Unity editor
- ❌ Si requiere localization futura, habría que refactor

**Mejora futura**: Scriptable Object "Rival" si escalamos a sistema de customización.

---

### 7. UI: Canvas Simple vs Prefab Modular

**Decisión**: **Canvas único** en cada escena con panels modularizados

**Justificación**:
- Cada escena es simple (MainMenu, Level01-06)
- Canvas por escena evita complejidad de pooling
- Panels (Intro, Result) son reutilizables con properties

**Estructura**:
```
Canvas
├── PanelIntro (CanvasGroup)
│   ├── TextRivalName
│   ├── TextNarrative
│   └── TextHint
├── PanelResult (CanvasGroup)
│   ├── TextResult
│   ├── ButtonNext
│   ├── ButtonRetry
│   └── ButtonMenu
└── HUDGameplay (CanvasGroup)
    └── TextRivalName (durante carrera)
```

**Trade-off**:
- ✅ Fácil de entender
- ✅ Canvas por escena = limpio
- ❌ Duplicación de Canvas entre escenas (negligible)
- ❌ Podría consolidarse en una escena persistente (future optimization)

---

## Decisiones Técnicas Específicas

### Movimiento Horizontal: Constante vs Acelerado

**Decisión**: **Velocidad constante 5 u/s** aplicada cada FixedUpdate

**Justificación**:
- Game es "endless runner" → velocidad infinita es parte del concepto
- Simplicity 🎯
- Arcade feel clasico (Excitebike, Flappy Bird)

```csharp
void FixedUpdate() {
    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);  // Override solo X
}
```

✅ NO aceleramos X
✅ NO aplicamos impulso X
✅ Override simple en cada frame

---

### Drag (Fricción): Modular vs Estática

**Decisión**: **Drag modular** según isGrounded

**Justificación**:
- Gameplay feel: jugador se siente "anclado" en suelo
- En aire: drift ligero (air drag < ground drag)

```csharp
void ApplyDrag() {
    rb.drag = isGrounded ? groundDrag : airDrag;
}
// groundDrag = 1.0 (suelo)
// airDrag = 0.5 (aire)
```

**Efecto**:
- Suelo: se detiene rápido si no corre
- Aire: mantiene inercia lateral (se siente responsive a querencia)

---

### Levels: Prefab vs Manualmente Construida

**Decisión**: **Construida manualmente por escena** (no procedural)

**Justificación**:
- MVP tiene solo 1-6 niveles
- Control fino de layout
- Testing determinístico
- Procedural generator sería overkill

**Estructura**:
- Cada `LevelXX.unity` tiene terreno pre-construido
- Obstacles/Platforms instanciadas de prefabs
- GameManager hace referencias en editor

---

### Pausa: Time.timeScale vs Local Timers

**Decisión**: **Time.timeScale = 0** cuando game termina

**Justificación**:
- Simple & standard en Unity
- Detiene todas las físicas automáticamente
- No requiere tracking manual de deltaTimes

```csharp
void EndGame(bool playerWon) {
    currentState = playerWon ? PlayerWon : RivalWon;
    Time.timeScale = 0f;  // Pausar TODO
    UIManager.instance.ShowResult(playerWon);
}
```

⚠️ **Cuidado**: UI buttons deben funcionar en timeScale 0 (update eventos UI ignora timeScale)

---

### Rendimiento: Objeto Pooling vs Instantiate

**Decisión**: **NO hacer pooling** (proyecto muy pequeño)

**Justificación**:
- Máximo 50 GameObjects activos simultáneamente
- Solo 1-2 level cargados a la vez
- Instantiate/Destroy es negligible

**Revisión**: Si en futuro necesitamos 100+ particles/bullets, considerar pooling.

---

## Posibles Problemas & Mitigaciones

### Problema: Jugador salta infinitamente en aire
**Causa**: Ground check falla (OverlapCircle no detecta)
**Mitigación**: 
- Debug.DrawCircle en GroundCheck
- Verificar que LayerMask "GroundCheck" está asignado
- Verificar offset Y es negativo

### Problema: Rival se atasca en obstáculos
**Causa**: Raycast no detecta, o física lo bloquea
**Mitigación**:
- Rival también debe tener colisiones normales (no solo triggers)
- Aumentar raycastDistance si es necesario
- Verificar que DecideJump se ejecuta cada frame

### Problema: UI no responde cuando Time.timeScale = 0
**Causa**: Unity UI tiene comportamiento especial pero algunos elementos pueden ignorarse
**Mitigación**:
- Usar `Update()` para UI (no FixedUpdate)
- O detectar pausa manualmente en UIManager

### Problema: FPS drops de 60 a 40 en ciertos puntos
**Causa**: Physics2D processing en frames específicos
**Mitigación**:
- Usar Profiler (Window → Analysis → Profiler)
- Verificar GC allocation
- Considerar bgroups/Rigidbody bodies en reposo

---

## Decisiones sobre Alcance

### ¿Por qué NO incluimos en MVP?

| Feature | Razón |
|---------|-------|
| Animaciones | Scope creep; Excitebike original era sprite simple |
| Sonido/Música | Proyecto académico sin requisito de audio |
| Coleccionables | Requiere rework UI y spawn system; futuro 1.1 |
| Multijugador | Scope completamente diferente |
| Compra de poder-ups | Complejidad en UI y balanceo |
| Leaderboards | Requiere persistencia/backend |
| Móvil/Web | Build target secundario |

---

## Checklist de Review Técnico

Antes de marcar una funcionalidad como DONE:

- [ ] Scripts compilados sin errores
- [ ] No hay warnings graves en Console
- [ ] Funcionalidad testeable (reproducible, no random)
- [ ] Performance: >=60 FPS en PC estándar
- [ ] Documentación actualizada
- [ ] Si afecta GDD, actualizar ese documento
- [ ] Si es feature nueva, agregar a CHANGELOG
- [ ] Entrada en DEVLOG con decisión tomada

---

**Última actualización**: 2026-04-05  
**Autor/a**: Asistente Técnico  
**Estado**: Documento vivo — sujeto a expansión conforme avance desarrollo

