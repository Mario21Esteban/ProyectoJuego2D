# TASKS — Checklist de Implementación MVP

**Tareas estructuradas para guiar el desarrollo del MVP.**
*Basado en GDD v1.1, Sección 2.6*

---

## FASE 1: Maqueta Digital (Semana 1, días 1-3)

**Objetivo**: Establecer el loop básico. Herramientas: Unity, scripts base.

### Setup del Proyecto
- [ ] Crear proyecto Unity 2022 LTS+ (2D Template)
- [ ] Crear estructura de carpetas:
  - [ ] Assets/Scripts/ (con subdirectorios: Player/, Rival/, Game/, UI/, Environment/)
  - [ ] Assets/Prefabs/ (con subdirectorios: Terrain/, UI/)
  - [ ] Assets/Scenes/
  - [ ] Assets/Resources/ (para narrative.json)
- [ ] Configurar Physics2D Settings:
  - [ ] Gravity: (0, -9.81)
  - [ ] Default Material: Friction 0.4, Bounce 0
- [ ] Crear Layers en Project:
  - [ ] Player
  - [ ] Rival
  - [ ] Terrain
  - [ ] Obstacle
  - [ ] Meta
  - [ ] GroundCheck
  - [ ] DeathZone

### Scripts Críticos (Fase 1)
- [ ] **PlayerController.cs**
  - [ ] Variables: moveSpeed, jumpForce, groundDrag, airDrag, groundLayer
  - [ ] FixedUpdate(): Movimiento horizontal (5 u/s)
  - [ ] Update(): Input Space para salto
  - [ ] Jump(): Aplicar impulso vertical
  - [ ] CheckGrounded(): OverlapCircle o Raycast
  - [ ] ApplyDrag(): Modular drag según isGrounded

- [ ] **RivalController.cs**
  - [ ] Variables: moveSpeed (4.5), jumpForce, decisionTimer, raycastDistance
  - [ ] FixedUpdate(): Movimiento horizontal (4.5 u/s)
  - [ ] Update(): Lógica de salto automático
  - [ ] DetectObstacle(): Raycast hacia adelante
  - [ ] DecideJump(): Lógica probabilística (70% default)

### Prefabs Básicos (Fase 1)
- [ ] **Player.prefab**
  - [ ] GameObject con Sprite Square (color azul #3B82F6)
  - [ ] Rigidbody2D (Dynamic, Mass 1, Gravity Scale 1, Freeze Rotation Z)
  - [ ] BoxCollider2D Main (0.8 × 1.5, No Trigger, Layer: Player)
  - [ ] BoxCollider2D GroundCheck (0.6 × 0.1, Offset -0.8Y, Trigger, Layer: GroundCheck)
  - [ ] SpriteRenderer configurado
  - [ ] Tag: "Player"
  - [ ] Script: PlayerController.cs

- [ ] **Rival.prefab** (copia de Player, color rojo #DC2626)
  - [ ] Posición inicial: (2, 0, 0)
  - [ ] Layer: Rival
  - [ ] Tag: "Rival"
  - [ ] Script: RivalController.cs

- [ ] **Ground.prefab**
  - [ ] Sprite Square escalado 10 × 0.5 (color verde #16A34A)
  - [ ] BoxCollider2D (No Trigger, Layer: Terrain)
  - [ ] Transform sin componentes script

### Pruebas Fase 1
- [ ] Crear escena "GameplayTest.unity"
- [ ] Instanciar Player y Rival
- [ ] Instanciar Ground simple
- [ ] Press Play:
  - [ ] ✓ Jugador corre a 5 u/s y salta con Space
  - [ ] ✓ Rival corre a 4.5 u/s
  - [ ] ✓ Ambos saltan sin chocar
  - [ ] ✓ Ground es colisionable
  - [ ] ✓ No hay errores en Console

**MILESTONE FASE 1**: Gameplay loop básico funcional sin UI

---

## FASE 2: Gameplay Funcional (Semana 1, días 4-5)

**Objetivo**: Mecánicas completas sin UI pulida.

### GameManager & State Machine
- [ ] **GameManager.cs**
  - [ ] Singleton pattern
  - [ ] Enum: GameState { Playing, PlayerWon, RivalWon }
  - [ ] Variables: playerPos, rivalPos, metaPos, deathYLevel (-20)
  - [ ] Update(): Chequear victoria/derrota cada frame
  - [ ] CheckVictory(): Si player.x >= meta.x → EndGame(true)
  - [ ] CheckDefeat(): Si rival.x >= meta.x OR player.y < -20 → EndGame(false)
  - [ ] EndGame(bool): Pausar time, cambiar state, notificar UIManager

### Terreno & Obstáculos
- [ ] **Platform.prefab** (plataforma elevada)
  - [ ] Sprite Square 3 × 0.5 (verde más claro #059669)
  - [ ] BoxCollider2D (No Trigger, Layer: Terrain)
  
- [ ] **Obstacle.prefab** (obstáculo dañino)
  - [ ] Sprite Square 0.5 × 0.5 (rojo oscuro #991B1B)
  - [ ] BoxCollider2D (Trigger, Layer: Obstacle)
  - [ ] Script: Obstacle.cs (OnTriggerEnter2D → GameManager.EndGame(false))

- [ ] **Meta.prefab** (línea de meta)
  - [ ] Sprite Square 1 × 3 (dorado #FBBF24)
  - [ ] BoxCollider2D (Trigger, Layer: Meta)
  - [ ] Script: Meta.cs (OnTriggerEnter2D → GameManager.CheckVictory())

- [ ] **DeathZone.prefab** (opcional, zona de caída)
  - [ ] BoxCollider2D grande debajo del mapa (Trigger, Layer: DeathZone)
  - [ ] Script opcional alternativo

### Diseño de Nivel 01
- [ ] Crear escena "Level01.unity"
- [ ] Layout simple (inspirado en GDD 9.1):
  - [ ] Ground base (0-80u)
  - [ ] Platform 1 (20-25u, altura 1u)
  - [ ] Platform 2 (45-48u, altura 2u)
  - [ ] Hueco (60-62u, ancho 2u)
  - [ ] Meta (80u)
  
- [ ] Instanciar Player, Rival, prefabs de terreno
- [ ] Bindear referencias en GameManager

### Pruebas Fase 2
- [ ] Play Level01:
  - [ ] ✓ Victoria: Jugador cruza meta primero
  - [ ] ✓ Derrota (rival): Rival cruza primero
  - [ ] ✓ Derrota (obstáculo): Tocar spike termina carrera
  - [ ] ✓ Derrota (caída): Saltar fuera mata jugador
  - [ ] ✓ Rival salta con timing realista

**MILESTONE FASE 2**: Un nivel completo jugable (win/lose funcional)

---

## FASE 3: Interacción & Control (Semana 2, días 1-3)

**Objetivo**: Menús, transiciones, narrativa.

### UI & Canvas
- [ ] Crear Canvas principal
  - [ ] Resolution: 1920x1080
  - [ ] Aspect Ratio: 16:9

### Scripts UI & Navigation
- [ ] **UIManager.cs**
  - [ ] Campos: introPanel, resultPanel, resultText, nextBtn, retryBtn, menuBtn
  - [ ] ShowResult(bool playerWon): Mostrar resultado, bindear botones
  - [ ] ShowIntroLevel(string rivalName, string intro): Mostrar narrativa
  - [ ] FadeTransition() coroutine: Animar alpha de CanvasGroup

- [ ] **LevelManager.cs**
  - [ ] Lista de LevelData (levelName, sceneName, rivalName, rivalSpeed)
  - [ ] currentLevelIndex
  - [ ] LoadLevel(int)
  - [ ] LoadNextLevel()
  - [ ] RestartLevel()
  - [ ] Patrón Singleton o DontDestroyOnLoad

### Menús
- [ ] **MainMenu.unity**
  - [ ] Título del juego
  - [ ] 4 botones: [JUGAR], [INSTRUCCIONES], [CRÉDITOS], [SALIR]
  - [ ] Background oscuro (#1a1a2e)

- [ ] **Intro Level Screen** (panel en-game)
  - [ ] Mostrar nombre rival
  - [ ] Mostrar texto narrativo (2-3 segundos)
  - [ ] "[PRESIONA SPACE PARA EMPEZAR]"
  - [ ] Canvas overlay con fade

- [ ] **Result Screen** (panel en-game)
  - [ ] Mostrar "¡GANASTE!" o "¡PERDISTE!"
  - [ ] Mensaje contextual: rival, obstáculo, caída, etc.
  - [ ] 3 botones: [SIGUIENTE], [REINTENTAR], [MENÚ]
  - [ ] Canvas overlay

### Narrativa
- [ ] **narrative.json** en Assets/Resources/
  - [ ] Estructura JSON con 6 niveles
  - [ ] Campos: level_id, intro, rival_name, victory, defeat
  - [ ] Script para cargar: Resources.Load<TextAsset>("narrative")

- [ ] Script: **NarrativeManager.cs** (opcional o integrado en UIManager)
  - [ ] Cargar JSON
  - [ ] Acceso por nivel

### Transiciones
- [ ] Fade in/out entre pantallas
- [ ] Transición MainMenu → Level01 Intro
- [ ] Transición resultado → siguiente nivel O reintentar
- [ ] Pausa visual clara (no saltos abruptos)

### Pruebas Fase 3
- [ ] Play desde MainMenu:
  - [ ] ✓ Botones navegan correctamente
  - [ ] ✓ [JUGAR] carga Level01 Intro
  - [ ] ✓ Intro muestra narrativa y espera Space
  - [ ] ✓ Gameplay corre después de intro
  - [ ] ✓ Resultado muestra victoria correcamente
  - [ ] ✓ [SIGUIENTE] carga Level02 (o placeholder)
  - [ ] ✓ [REINTENTAR] recarga Level01
  - [ ] ✓ [MENÚ] vuelve a MainMenu

**MILESTONE FASE 3**: Juego navegable de menú a resultado

---

## FASE 4: UI & Polish (Semana 2, días 4-5)

**Objetivo**: Producto presentable.

### Visual Refinement
- [ ] Sprites finales (o placeholder mejorados):
  - [ ] Player azul con detalle mínimo
  - [ ] Rival rojo con ID visual (si aplica)
  - [ ] Terreno con colores consistentes
  - [ ] Obstáculos distinguibles

- [ ] Animaciones triviales (opcional pero recomendado):
  - [ ] Color flash cuando toca obstáculo
  - [ ] Fade suave de pantallas
  - [ ] Bounce del jugador al saltar (sprite squash)

### UI Gameplay
- [ ] HUD minimalista durante carrera:
  - [ ] Nombre del rival visible (esquina superior)
  - [ ] Posición relativa (opcional, puede ser solo visual)
  - [ ] Sin clutter

- [ ] Feedback visual:
  - [ ] Pantalla roja al perder
  - [ ] Pantalla dorada/brillante al ganar

### Rendimiento & Optimización
- [ ] Verificar que corre a ~60 FPS (target)
- [ ] Usar Profiler si hay stutters
- [ ] Revisar memory usage (target < 512 MB)
- [ ] No debe haber memory leaks visibles

### Documentación de Código
- [ ] Comentarios en scripts clave
- [ ] Resumen de métodos principales
- [ ] Relaciones entre sistemas documentadas

### Pruebas Fase 4
- [ ] Testing exhaustivo:
  - [ ] ✓ Sin crashes durante 5 minutos de gameplay
  - [ ] ✓ FPS estable en 60
  - [ ] ✓ Transiciones suaves
  - [ ] ✓ Narrativa se carga correctamente
  - [ ] ✓ UI es legible y responsive

- [ ] Testing en múltiples PCs (si es posible)

**MILESTONE FASE 4**: MVP presentable y pulido

---

## FASE 5: Expansión Condicional (Semana 3-4, si hay tiempo)

**Objetivo**: Agregar 3-6 niveles completos. SOLO SI MVP está estable.

### Diseño de Niveles 02-06
- [ ] **Level02.unity** (Ana - Técnica, Dificultad ⭐⭐)
  - [ ] Rival speed: 4.5 u/s
  - [ ] Layout: Plataformas alternadas + rampa
  - [ ] Complejidad moderada

- [ ] **Level03.unity** (Luis - Veterano, Dificultad ⭐⭐⭐)
  - [ ] Rival speed: 4.7 u/s
  - [ ] Layout: Múltiples huecos y obstáculos
  
- [ ] **Level04.unity** (Sofía - Rival Local, ⭐⭐⭐⭐)
  - [ ] Rival speed: 4.85 u/s
  - [ ] Layout: Complejidad alta

- [ ] **Level05.unity** (Mario - Ídolo, ⭐⭐⭐⭐⭐)
  - [ ] Rival speed: 4.95 u/s
  - [ ] Layout: Muy desafiante

- [ ] **Level06.unity** (UNIDAD - Campeón, ⭐⭐⭐⭐⭐⭐)
  - [ ] Rival speed: 5.0 u/s (perfección)
  - [ ] Layout: Extremadamente desafiante

### Balanceo de IA por Nivel
- [ ] Ajustar parámetros RivalController según tabla GDD 7.2:
  - [ ] Velocidad
  - [ ] decisionTimer (más frecuente = más saltos)
  - [ ] jumpProbability

### Progresión Visual
- [ ] Backgrounds pueden escalar en complejidad (opcional)
- [ ] Dificultad visual progresiva

### Testing Expansión
- [ ] Jugar todos los 6 niveles en secuencia
- [ ] Verificar balance de dificultad
- [ ] Speed run testing: ¿tiempo estimado total?

**MILESTONE FASE 5**: MVP completo con 6 niveles (OPCIONAL)

---

## QA & RELEASE

### Testing Final Completo
- [ ] **Test 1 - Tutorial Implícito**
  - [ ] ✓ Sin leer instrucciones, ¿entiendo la mecánica?

- [ ] **Test 2 - Victoria**
  - [ ] ✓ Con timing perfecto, ¿llegó primero a meta?

- [ ] **Test 3 - Derrota por Obstáculo**
  - [ ] ✓ ¿Registra derrota al tocar spike?

- [ ] **Test 4 - Derrota por Caída**
  - [ ] ✓ ¿Detecta caída fuera de pista?

- [ ] **Test 5 - Rival Ganador**
  - [ ] ✓ ¿Rival gana si no salto?

- [ ] **Test 6 - Flujo Completo**
  - [ ] ✓ Menú → Nivel → Victoria → Siguiente → Menú (sin errores)

- [ ] **Test 7 - Rendimiento**
  - [ ] ✓ 5 minutos sin stutters
  - [ ] ✓ FPS estable 60
  - [ ] ✓ Sin memory leaks

### Documentación Final
- [ ] README.md actualizado
- [ ] Código comentado
- [ ] DEVLOG final con lecciones aprendidas

### Build Final
- [ ] Build para Windows 64-bit
- [ ] Executable funcional
- [ ] Assets empaquetadas correctamente

---

## LEYENDA

| Símbolo | Significado |
|---------|------------|
| [ ] | Tarea no completada |
| [x] | Tarea completada |
| ✅ | Tarea con éxito confirmado |
| ❌ | Tarea bloqueada o fallida |
| ⏳ | Tarea en progreso |
| 📌 | Nota importante |
| ⏭️ | Próximo action item |

---

**Última actualización**: 2026-04-05  
**Estado actual**: Listo para comenzar FASE 1

