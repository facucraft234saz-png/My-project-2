# Trabajo Practico 01 / Programación de Videojuegos I

## Descripción

Este proyecto corresponde al **Trabajo Práctico N.º 01 de Programación de Videojuegos I**.

El objetivo del ejercicio fue desarrollar un **prototipo de videojuego 3D de obstáculos utilizando Unity**, aplicando diferentes conceptos de programación y desarrollo de videojuegos.

El jugador debe recorrer un circuito de plataformas, superar obstáculos, utilizar plataformas móviles y recoger una caja/objeto para transportarla hasta una zona de entrega (meta).

Durante el recorrido también puede encontrar un **Power-Up temporal de velocidad y salto**, que aumenta las capacidades del jugador durante un período determinado.

El proyecto permite poner en práctica conceptos como **movimiento, físicas, colisiones, prefabs, corrutinas, temporizadores, generación de objetos y manipulación de la jerarquía de objetos**.

<img width="535" height="232" alt="image" src="https://github.com/user-attachments/assets/d3fc66c8-d9e3-4c3f-af03-eb54daee1cfe" />


---

## Versión de Unity

Unity 6 

---

## Controles

| Tecla | Acción |
| :--- | :--- |
| **WASD** | Movimiento del personaje |
| **Espacio** | Saltar |
| **Flecha Izquierda / Derecha** | Rotar / Mover la cámara |
| **E** | Agarrar la caja |
| **Q** | Soltar la caja |

---

## Mecánicas implementadas

* **Seguimiento y control de cámara:** Control del ángulo de visión durante el recorrido del circuito.
* **Movimiento y salto del jugador:** Control con físicas y detección estricta de suelo.
  
<img width="766" height="322" alt="image" src="https://github.com/user-attachments/assets/66957ebe-dc2c-4680-8c15-da1d536b8bb5" />


* **Plataformas móviles entre dos posiciones:**
  * Sincronización del movimiento del jugador sobre la plataforma.
  
<img width="525" height="269" alt="image" src="https://github.com/user-attachments/assets/fb48260b-1378-4d3a-aea3-efb8cd7618ec" />


* **Generación periódica de obstáculos mediante `InvokeRepeating()`:**
  * Los obstáculos/proyectiles se desplazan hacia el jugador.
  * Autodestrucción de proyectiles por tiempo (`Destroy`).
  * Los obstáculos teletransportan al jugador o la caja al colisionar.
  
<img width="443" height="213" alt="image" src="https://github.com/user-attachments/assets/a581101d-7db0-41cc-8cd5-49a1568c4b99" />


* **Recolección y transporte de la caja mediante jerarquía (`SetParent`):**
  * Soltar la caja y devolverla a su estado físico normal.
  
<img width="286" height="113" alt="image" src="https://github.com/user-attachments/assets/d55b33ca-a306-497c-a5c1-8e745ea879f8" />

* **Power-Up temporal de velocidad y salto mediante una corrutina:**
  * Aumento del 200% en atributos con reaparición (*respawn*) de la esfera.
  
<img width="497" height="176" alt="image" src="https://github.com/user-attachments/assets/7f6e5a60-fae6-4592-8692-56c8749108a9" />


* **Zona de entrega (GoalZone):**
  * Detección mediante *Triggers* y consulta del estado de la caja.
  * Condición de victoria activa solo si se lleva el objeto.
  * Cambio de color de la meta y mensaje en pantalla UI (`TextMeshProUGUI`).
  
<img width="570" height="292" alt="image" src="https://github.com/user-attachments/assets/d84460e3-f2df-470f-81aa-0423be56d568" />


---

## Conceptos de programación utilizados

Durante el desarrollo del proyecto se utilizaron diferentes herramientas y conceptos de Unity:

* **Variables y referencias mediante el Inspector.**
* **Métodos y estructuras de control.**
* **`InvokeRepeating()`** para generar obstáculos periódicamente.
* **`Instantiate()`** para crear objetos durante la ejecución.
* **`Destroy()`** para eliminar obstáculos después de un tiempo.
* **Corrutinas (`IEnumerator`)** para controlar efectos temporales y reaparición de objetos.
* **Transform e Interacciones de Jerarquía** para transportar la caja junto al jugador.
* **Triggers y colisiones (`OnTriggerEnter`, `OnCollisionEnter`)** para detectar interacciones.
* **Rigidbody y Time.deltaTime / fixedDeltaTime** para trabajar con físicas uniformes.
* **Prefabs** para reutilizar elementos del escenario (Spawner, Proyectil, Power-Up).

---

## Objetivo del juego

El objetivo principal es **recorrer el circuito, superar los obstáculos, recoger la caja y llevarla hasta la zona de entrega**.

Para completar el recorrido, el jugador deberá utilizar las plataformas disponibles, evitar los proyectiles generados y aprovechar el Power-Up de velocidad/salto.

Al entregar correctamente la caja en la zona correspondiente, se activa el **evento de victoria**, mostrando un mensaje en pantalla y cambiando el color de la meta como señal de que el objetivo fue completado.

---

## Tareas y mecánicas completadas

- [x] Movimiento del jugador y salto.
- [x] Controles de cámara.
- [x] Plataformas móviles.
- [x] Generación de obstáculos mediante `InvokeRepeating()`.
- [x] Destrucción automática de obstáculos.
- [x] Recolección y transporte de la caja.
- [x] Soltar la caja.
- [x] Power-Up temporal de velocidad y salto.
- [x] Reaparición (respawn) del Power-Up.
- [x] Zona de entrega (GoalZone) condicional.
- [x] Mensaje de victoria e interfaz UI.
- [x] Cambio de color de la meta al ganar.
---
Autor
[Robert Fuentes Facundo Guillermo] - DNI: [46597991]
---

## Ejemplo de código

Una de las herramientas utilizadas en el proyecto fue `InvokeRepeating()`, utilizada para generar obstáculos de manera periódica:

```csharp
void Start()
{
    // Llama a SpawnProjectile repetidamente cada 'spawnTime' segundos
    InvokeRepeating(nameof(SpawnProjectile), 0f, spawnTime);
}

