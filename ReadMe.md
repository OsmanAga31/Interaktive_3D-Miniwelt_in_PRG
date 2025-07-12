
# Dokumentation  
**Dragon Ball – TicTacToe auf der Kame-Insel**  
Unity 6 – Stand 12. Juli 2025

---

## 1. Spielidee & Narrative  
Der Spieler erwacht verwirrt am Strand einer cartoonhaften Insel. Er stammt aus der „echten Welt“ und landet unfreiwillig in dieser skurrilen Umgebung. Der alte Meister **Roshi** bittet ihn, die **sieben Dragon Balls** zu suchen. Nach dem Sammeln und der Beschwörung des Drachen **Shenlong** erwartet den Spieler keine große Erfüllung, sondern lediglich ein **TicTacToe-Set**, mit dem er nun gegen Roshi antritt.

---

## 2. Features & Gameplay  
| Funktion | Beschreibung |
|---|---|
| **Interaktion mit E** | Grabben der Dragon Balls, Gespräche mit Roshi, Betreten des Kame-Hauses – alles über die **E-Taste**. |
| **Grab-System** | Dragon Balls lassen sich anvisieren, aufheben und vor sich herziehen. |
| **Dynamische Dialoge** | • Vor Sammelabschluss: 2 Voice-Lines von Roshi<br>• Nach Sammeln, vor Beschwörung: 1 Voice-Line<br>• Nach Shenlong: Roshi lädt zum TicTacToe ein. |
| **TicTacToe-Partien** | • Spielfeld erscheint nach Drachenbeschwörung<br>• Roshi kommentiert **nur den ersten Zug des Spielers in jeder Runde** (3 mögliche Sprüche)<br>• Sieg / Niederlage wird live in der UI gezählt. |
| **Zweiländer-Hub** | Zwei Inseln über Stege verbunden; am Steg-Ende katapultieren "**Jump-Pads**" den Spieler hin & her. |
| **Quest-Tracker** | Oben rechts: aktuelle Aufgabe + TicTacToe-Statistik (Siege / Niederlagen). |
| **Kame-Haus Showroom** | Im Inneren befindet sich eine **begehbare Ausstellung** der selbst erstellten Assets; die **Easteregg-Statue** steht jedoch in der **Hauptszene** beim Kame-Haus. |
| **Sicherheitsfunktion** | Läuft der Spieler (oder ein Dragon Ball) zu weit ins Meer, wird er / es sofort auf die Hauptinsel bzw. zurück zur Schüssel neben Roshi teleportiert. |

---

## 3. Verwendete Tools  
| Tool | Zweck | Link |
|---|---|---|
| **Unity 6** | Engine, Szenebau, UI, Gameplay-Logik | – |
| **Visual Studio** | C#-Skripting (Selbst & Tab-Completion); **nur `JumpPad.cs` wurde vollständig von GitHub Copilot erzeugt** | – |
| **Blender** | • **Separieren** der Dragon Balls (nicht modelliert)<br>• **Modellierung eigener Assets** (siehe unten) | – |
| **Substance Painter** | Texturierung der meisten Eigen-Assets | – |
| **Mixamo** | Auto-Rigging & Idle-Animation für „Maestro Roshi“ | – |
| **Sketchfab / Unity Asset Store** | Beschaffung der meisten 3D-Assets | siehe Abschnitt 4 |
| **Google AI Studio TTS** | Sprachsynthese **aller** gesprochenen Inhalte (Roshi, Spieler, Shenlong) | [aistudio.google.com/generate-speech](https://aistudio.google.com/generate-speech) |
| **FakeYou** | Voice-Cloning für Shenlong | [fakeyou.com/weight/…/shenron](https://fakeyou.com/weight/weight_kyt5h1hrbcb5wmsjhg189j411/shenron) |
| **Kimi K2** | Struktur und Wortlaut **dieser Dokumentation** | [github.com/MoonshotAI/Kimi-K2](https://github.com/MoonshotAI/Kimi-K2) |
| **PlayerController** | Grundgerüst gemeinsam mit **Lehrer Bohdan Chumak** erstellt, anschließend **eigenständig adaptiert** | – |
| **Kommentare & Inline-Docs** | Teilweise über **Claude 4** auf claude.ai, restliche direkt in **Visual Studio Copilot** | – |

---


## 4. Asset-Liste (inkl. Links)  
| Asset | Herkunft | Hinweis |
|---|---|---|
| **Maestro Roshi** | [Sketchfab](https://sketchfab.com/3d-models/maestro-roshi-4d77c491981f4733aa093902430ba00b) | Aktives NPC-Modell (Mixamo-Rig) |
| **Roshi_DecimatedTest** | [Sketchfab](https://sketchfab.com/3d-models/roshi-decimatedtest-90aFt2SlTzXnlWkXPrzagRaCdPX) | Easteregg-Statue in der Hauptszene |
| **Kame House** | [Sketchfab](https://sketchfab.com/3d-models/kame-house-f055f8adf89c4d1280b2bfa8edb49a7c) | Haus-Modell – das mitgelieferte Meer wurde **ausgeblendet** |
| **Dragon Balls** | [Sketchfab](https://sketchfab.com/3d-models/dragon-balls-efdc852cd4cd4e91b7658a201c47752a) → Blender | Nur **separiert**, nicht modelliert |
| **Dragon Ball Z SHENLONG** | [Sketchfab](https://sketchfab.com/3d-models/dragon-ball-z-shenlong-ee561e95bd9642f193fd09e8058f6c2f) | Ursprüngliches Modell, nachträglich mit **Inverse Kinematics (IK)** versehen, um die spezielle Pose im Spiel zu erzielen. |
| **Ocean & Lake Shaders (URP)** | [Unity Asset Store](https://assetstore.unity.com/packages/vfx/shaders/ocean-lake-shaders-urp-303983) | **Meer** |
| **Low-Poly Tropical Island Lite** | [Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/low-poly-tropical-island-lite-242437) | Stege |
| **Free Quick Effects Vol. 1** | [Unity Asset Store](https://assetstore.unity.com/packages/vfx/particles/free-quick-effects-vol-1-304424) | Jump-Pad-Leuchteffekt, Beschwörungsrauch |
| **FastSky – Procedural Sky** | [Unity Asset Store](https://assetstore.unity.com/packages/vfx/shaders/fastsky-procedural-sky-and-clouds-urp-209857) | Skybox – **Sonnenrotation per eigenem Script**, visuelle Tageszeiten vom Asset übernommen |

---

## 5. Selbst erstellte & texturierte Assets  
- **BowlForDragonballs** & **Cushion for the bowl**  
- **Door**  
- **Flower**  
- **Island (Beschwörungsort)**  
- **Podest**  
- **Seashell**  
- **Showroom**  
- **TicTacToe Set**

---

## 6. Besonderheiten & KI-Einsatz  
- **KI-generierte Inhalte**:  
  – `JumpPad.cs` (GitHub Copilot)  
  – alle gesprochenen Texte & Audiodateien (Google TTS / FakeYou)  
  – **Layout & Struktur dieser Dokumentation (Kimi K2)**

- **Zwei Szenen**:  
  – **Hauptinsel** (inkl. Kame-Haus + Easteregg-Statue)  
  – **Showroom** (begehbare Ausstellung der Eigen-Assets)

- **Sicherheitsfunktion**: Teleport bei Wasser-Kontakt verhindert Verlust von Spieler oder Dragon Balls.
