# Proyecto: Runner 2D Academia

**Prototipo académico de videojuego 2D en Unity (estilo Excitebike / endless runner lateral con lógica de plataformas)**

---

## 📋 Información General

| Atributo | Valor |
|----------|-------|
| **Título** | Runner 2D Academia |
| **Género** | Runner 2D / Arcade Racing |
| **Engine** | Unity 2022 LTS+ |
| **Lenguaje** | C# |
| **Plataforma** | Windows (PC) |
| **Estado** | MVP (Fase 1 - Maqueta) |
| **Equipo** | 1 developer (académico) |
| **Duración estimada** | 2-4 semanas |

---

## 🎮 Concepto

Un piloto novato compite en carreras 2D contra rivales progresivamente más difíciles. 

**Mecánica de juego**:
- Corre automáticamente hacia la derecha
- **Salta** con SPACE para evitar obstáculos y terreno irregular
- **Compite 1v1** contra un rival automático por nivel
- **Gana** siendo el primero en llegar a la meta
- **Pierde** si tocas un obstáculo, caes fuera de pista, o el rival gana

**Requisitos académicos cumplidos**:
- ✅ Movimiento (horizontal automático + salto vertical)
- ✅ Salto con físicas reales (gravedad, impulso)
- ✅ Plataformas (segmentos elevados de pista)
- ✅ Colisiones (ground check, obstáculos, triggers)
- ✅ Daño (obstáculos = derrota instantánea)
- ✅ UI (menús, resultado, narrativa)
- ✅ Narrativa (contexto de rivales por nivel)
- ✅ Física 2D integrada

Ver **[Docs/GDD.md](Docs/GDD.md)** para especificación completa.

---

## 📁 Estructura del Proyecto

```
ProyectoJuego2D/
├── Assets/
│   ├── Scripts/              # Código fuente (C#)
│   │   ├── Player/           # PlayerController.cs
│   │   ├── Rival/            # RivalController.cs
│   │   ├── Game/             # GameManager, LevelManager
│   │   ├── UI/               # UIManager
│   │   └── Environment/      # Obstacle.cs, Meta.cs
│   │
│   ├── Prefabs/              # GameObjects reutilizables
│   │   ├── Player.prefab
│   │   ├── Rival.prefab
│   │   └── Terrain/          # Ground, Platform, Obstacle, Meta
│   │
│   ├── Scenes/               # Escenas Unity
│   │   ├── MainMenu.unity
│   │   ├── GameplayTest.unity
│   │   ├── Level01.unity
│   │   └── Level02-06.unity
│   │
│   └── Resources/
│       └── narrative.json    # Diálogos y narrativa por nivel
│
├── Docs/
│   ├── GDD.md                # Game Design Document (especificación completa)
│   ├── CHANGELOG.md          # Registro de versiones
│   ├── DEVLOG.md             # Diario de desarrollo por sesión
│   ├── TASKS.md              # Checklist de tareas por fase
│   └── TECHNICAL_NOTES.md    # Decisiones arquitectónicas
│
└── README.md                 # Este archivo
```

---

## 🚀 Quick Start (30 min)

### Paso 1: Setup del Proyecto
1. Abre **Unity 2022 LTS+**
2. Abre la carpeta `ProyectoJuego2D`
3. En Project Settings → Physics2D:
   - Gravity: (0, -9.81)

### Paso 2: Crear Layers
En Project Settings → Tags and Layers, crear:
- Player, Rival, Terrain, Obstacle, Meta, GroundCheck, DeathZone

### Paso 3: Primeros GameObjects
1. Crear Sprites Square para Player (azul) y Rival (rojo)
2. Agregar Rigidbody2D y BoxColliders
3. Crear Ground simple (verde, 10×0.5)

### Paso 4: Scripting
Ver **[Apéndice A del GDD](Docs/GDD.md#apéndice-a-quick-start-guide--primeros-pasos-30-min)** para templates de código.

**[Ver Docs/TASKS.md para checklist técnico detallado]**

---

## 📖 Documentación

| Archivo | Propósito |
|---------|-----------|
| **[GDD.md](Docs/GDD.md)** | Especificación completa: mecánicas, niveles, narrativa, sistemas técnicos, requisitos académicos |
| **[CHANGELOG.md](Docs/CHANGELOG.md)** | Historial de versiones y cambios importantes |
| **[DEVLOG.md](Docs/DEVLOG.md)** | Diario de sesiones de trabajo (qué se hizo, problemas, decisiones) |
| **[TASKS.md](Docs/TASKS.md)** | Checklist completo de implementación por fase (use para tracking) |
| **[TECHNICAL_NOTES.md](Docs/TECHNICAL_NOTES.md)** | Decisiones arquitectónicas: por qué se eligieron ciertos enfoques |

**Flujo recomendado**:
1. Leer **GDD.md** sección 1-3 (visión + mecánicas)
2. Revisar **TASKS.md** para saber qué hacer
3. Consultar **TECHNICAL_NOTES.md** mientras codificas
4. Actualizar **DEVLOG.md** y **CHANGELOG.md** al completar hitos

---

## 🎯 Fases de Desarrollo

El proyecto se divide en **5 fases** (basadas en GDD v1.1, Sección 2.5):

### FASE 1: Maqueta Digital (Semana 1, días 1-3)
**Objetivo**: Loop básico game (jugador + rival + terreno running)
- Setup de proyecto, scripts base, prefabs simples
- **Resultado**: Jugador corre y salta; rival corre; colisiones funcionan

### FASE 2: Gameplay Funcional (Semana 1, días 4-5)
**Objetivo**: Mecánicas completas sin UI
- GameManager (victoria/derrota), Level01 con terreno
- **Resultado**: Un nivel jugable (win/lose)

### FASE 3: Interacción & Control (Semana 2, días 1-3)
**Objetivo**: Menús, narrativa, transiciones
- MainMenu, intro level, result screen, LevelManager
- **Resultado**: Juego navegable menú → gameplay → resultado

### FASE 4: UI & Polish (Semana 2, días 4-5)
**Objetivo**: Producto presentable
- Refinamiento visual, optimización, testing exhaustivo
- **Resultado**: MVP listo para presentación

### FASE 5: Expansión Condicional (Semana 3-4, si  tiempo)
**Objetivo**: 6 niveles completos
- Nivel 01-06 con dificultad progresiva, balanceo
- **Resultado**: Juego completo (OPCIONAL si MVP estable)

**Usa [TASKS.md](Docs/TASKS.md) para marcar progreso.**

---

## 🛠️ Tecnología

### Stack
- **Engine**: Unity 2022 LTS+ (2D)
- **Código**: C# (.NET Standard 2.1)
- **Physics**: Physics2D nativa
- **UI**: Canvas + EventSystem
- **Persistencia**: JSON (narrativa)

### Requisitos del Sistema
- **OS**: Windows 10/11 64-bit
- **RAM**: 4GB mínimo, 8GB recomendado
- **GPU**: Compatible con Direct3D 11+

---

## 📊 Métricas de Éxito

El MVP es exitoso cuando:

```
✓ Jugador avanza, salta, y no atraviesa terreno
✓ Rival compite con IA autónoma
✓ Sistema victoria/derrota funciona
✓ UI menús y transiciones son fluidas
✓ Juego corre a 60 FPS sin stutters
✓ No hay crasheshort
✓ Un nivel es jugable completamente
✓ Narrativa se carga correctamente
✓ Código es legible y documentado
```

**Criterios académicos**: Ver [GDD.md Sección 12](Docs/GDD.md#12-métricas-de-éxito-y-criterios-de-evaluación).

---

## 🐛 Troubleshooting Rápido

| Problema | Solución |
|----------|----------|
| **Jugador cae a través del suelo** | Verificar que Ground tiene BoxCollider2D (Is Trigger = false) y Layer "Terrain" |
| **Jugador salta infinitamente** | Chequear Ground Check setup (OverlapCircle con offset correcto) |
| **Rival no salta** | Debug.Log en DecideJump(); revisar isGrounded |
| **FPS baja** | Usar Profiler (Window → Analysis); revisar cantidad de objetos |
| **Narrativa no carga** | Verificar path `Assets/Resources/narrative.json` y validar JSON |

**Más ayuda**: Ver [TECHNICAL_NOTES.md](Docs/TECHNICAL_NOTES.md#posibles-problemas--mitigaciones) y [GDD.md Apéndice C](Docs/GDD.md#apéndice-c-troubleshooting-común).

---

## 📝 Cómo Contribuir a la Documentación

Cada sesión de trabajo debe actualizar:

1. **DEVLOG.md**: Qué se hizo, problemas, próximo paso
2. **TASKS.md**: Marcar tareas completadas
3. **CHANGELOG.md**: Si hay cambio importante de versión
4. **TECHNICAL_NOTES.md**: Si se toma una decisión técnica relevante
5. **GDD.md**: SOLO si cambia mecánica/diseño (no para cambios de código)

**Ejemplo**:
```
Sesión: 2026-04-10 — Implementar PlayerController

✅ Trabajo realizado:
- Playercontroller.cs con movimiento + salto
- Ground check con OverlapCircle funcionando
- Aplicación de drag basado en estado

⏳ TASKS.md:
- [x] PlayerController.cs
- [x] CheckGrounded()
- [ ] RivalController.cs (próximo)

📝 DEVLOG.md:
- Agregó entrada nueva con decisiones tomadas
```

---

## 🎓 Para Evaluadores

Este proyecto demuestra:

- **Programación OOP**: Controllers como clases independientes
- **Physics 2D**: Gravedad real, cálculo de impulsos, colisiones
- **State Management**: GameState enum, transiciones claras
- **UI**: Canvas, botones, transiciones animadas
- **Game Design**: Narrativa, balance de dificultad, loop de juego
- **Documentación**: GDD, DEVLOG, CHANGELOG integrados
- **Version Control**: Estructura de código modular y escalable

Ver [GDD.md Sección 12](Docs/GDD.md#12-métricas-de-éxito-y-criterios-de-evaluación) para criterios académicos completos.

---

## 📞 Contacto / Soporte

Si tienes dudas durante desarrollo:

1. Consulta el **GDD.md** (especificación oficial)
2. Revisa **TECHNICAL_NOTES.md** (decisiones arquitectónicas)
3. Chequea **DEVLOG.md** para ver si alguien enfrentó el mismo problema
4. Busca en **Docs/GDD.md Apéndice C** (troubleshooting)

---

## 📌 Estado Actual

| Elemento | Estado |
|----------|--------|
| **Documentación** | ✅ Completa (GDD v1.1, CHANGELOG, DEVLOG, TASKS, TECHNICAL_NOTES) |
| **Setup de Proyecto** | ⏳ Pendiente (FASE 1) |
| **Código Base** | ⏳ Pendiente (FASE 1) |
| **Gameplay Loop** | ⏳ Pendiente (FASE 1-2) |
| **UI / Menús** | ⏳ Pendiente (FASE 3) |
| **MVP Completo** | ⏳ Pendiente (FASE 4) |
| **6 Niveles (opcional)** | ⏳ Pendiente (FASE 5) |

**Siguiente paso**: Comenzar **FASE 1** (crear proyecto Unity + PlayerController).

---

## 📋 Cambios Recientes

- **2026-04-05**: v1.1 GDD actualizado; creación de sistema de documentación completo (CHANGELOG, DEVLOG, TASKS, TECHNICAL_NOTES)

---

**Proyecto iniciado**: 2026-03-24  
**Última actualización**: 2026-04-05  
**Versión**: 1.1 (MVP Spec)  
**Licencia**: Académico (uso interno)

