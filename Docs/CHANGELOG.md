# CHANGELOG — Proyecto Runner 2D Academia

**Registro de versiones y cambios significativos.**

---

## [1.1] - 2026-04-05

### Añadido
- **Alineación académica explícita** (Sección 2.4 del GDD)
  - Tabla de mapeo: Requisitos académicos → Mecánicas del juego
  - Demuestra cumplimiento de 9/10 requisitos en MVP
  
- **Fases de desarrollo estructuradas** (Sección 2.5)
  - 5 fases claras desde maqueta hasta expansión
  - Estimaciones de tiempo por fase
  - Milestones verificables

- **6 Sistemas técnicos definidos** (Sección 8.3)
  - PlayerController, RivalController, GameManager, LevelManager, UIManager
  - Obstacle & Meta scripts
  - Cada sistema con responsabilidad, pseudocódigo y métodos

- **Checklist de implementación** (Sección 2.6)
  - 17 scripts necesarios
  - 7 prefabs esenciales
  - 4 escenas base
  - Setup Unity completo

- **Métricas de éxito académicas** (Sección 12)
  - Checklist de aceptación MVP (17 items)
  - 8 competencias demostrables
  - Plan de testing (7 tests críticos)
  - 7 criterios de aprobación académica

- **6 Apéndices prácticos**
  - A: Quick Start Guide (30 min)
  - B: Glosario (13 términos)
  - C: Troubleshooting (5 problemas)
  - D: Diagrama visual gameplay loop
  - E: Timeline realista (Semana 1-2)
  - F: Recursos externos recomendados

- **Plantillas de código** (Sección 17)
  - PlayerController skeleton
  - GameManager skeleton con lógica completa

### Modificado
- **Concepto de una línea** — Más específico sobre interpretación académica
- **Sección 1.3** → Renombrada a 1.4 (Elevator Pitch)
- **Estructura general** — Reordenamiento para flujo lógico: Visión → Requisitos Académicos → Fases → Sistemas → Métricas

### Corregido
- Aclaraciones en definiciones de términos técnicos
- Ampliación de detalles en configuración de GameObjects

---

## [1.0] - 2026-03-24

### Añadido
- **Visión general completa**
  - Concepto, género (Runner 2D arcade-style)
  - Público objetivo (académico)
  - Plataforma (Windows PC, Unity 2022 LTS+)

- **Descripción del juego**
  - Loop de gameplay principal
  - Flujo de juego completo (menu → intro → gameplay → resultado)
  - Duración estimada

- **Mecánicas principales**
  - Movimiento automático (5 u/s)
  - Salto controlado (Space)
  - Sistema de carrera 1v1
  - Condiciones de victoria/derrota

- **Narrativa & Progresión**
  - 6 rivales definidos (Carlos, Ana, Luis, Sofía, Mario, UNIDAD)
  - Estructura de 3 actos
  - Textos de intro por nivel

- **Gameflow & Menús**
  - Diagrama de flujo completo
  - Descripción de 5 pantallas principales

- **Especificaciones Técnicas**
  - Configuración Unity (resolución, FPS, physics2D)
  - Layers y collision matrix
  - Arquitectura inicial de scripts
  - Configuración de GameObjects

- **IA del Rival**
  - Algoritmo de movimiento
  - Parámetros por nivel
  - Tabla de ajustes de dificultad

- **Referencia Rápida** (Sección 14-15)
  - Resumen de mecánicas
  - Duración estimada del proyecto

---

## Notas sobre versionado

- **v1.0 → v1.1**: Mejora académica; sin cambios en mecánicas del juego
- **Próximo cambio esperado**: v1.2 (si se detectan cambios en GDD durante desarrollo) o v2.0 (expansión a 6 niveles completos)

---

**Última actualización**: 2026-04-05
