# 🎮 B-Boy Aliens

Jogo 3D cooperativo mobile inspirado na cultura Hip-Hop, onde dois alienígenas utilizam movimentos de B-Boy e música para restaurar a cultura na Terra.

---

## 📦 Requisitos

Antes de executar o projeto, certifique-se de ter instalado:

* Unity Hub
* Unity (versão recomendada: **2022 LTS ou superior**)
* Visual Studio / VS Code
* Git (opcional)

### Dependências do Projeto:

* Photon Fusion (Multiplayer)
* Input System (Unity)
* URP (Universal Render Pipeline)

---

## 🚀 Como Executar o Projeto

### 1. Clonar o repositório

```bash
git clone <URL_DO_REPOSITORIO>
```

### 2. Abrir no Unity

* Abra o **Unity Hub**
* Clique em **"Open Project"**
* Selecione a pasta do projeto

### 3. Instalar dependências

Caso o Unity solicite:

* Instale pacotes automaticamente via **Package Manager**
* Verifique se o Photon Fusion está importado

---

## ▶️ Executar o jogo

### No Editor:

1. Abra a cena principal:

```
Assets/Scenes/MainScene.unity
```

2. Clique em **Play**

---

## 📱 Build para Android

### 1. Configurar plataforma

* Vá em:

```
File → Build Settings
```

* Selecione **Android**
* Clique em **Switch Platform**

### 2. Configurar Player Settings

* Defina:

  * Package Name
  * Minimum API Level
  * Permissões (Internet)

### 3. Gerar build

* Clique em **Build** ou **Build and Run**

---

## 🌐 Multiplayer (Photon Fusion)

Para funcionamento do multiplayer:

1. Criar conta no Photon:
   👉 https://www.photonengine.com/

2. Criar uma aplicação (Fusion)

3. Copiar a **App ID**

4. No Unity:

* Acesse:

```
Photon > Fusion > Fusion Hub
```

* Cole a App ID no projeto

---

## 🎮 Controles (Mobile)

* Movimento: Joystick virtual
* Ataque: Botão de ação
* Uso de item: Botão secundário

---

## ⚠️ Problemas Comuns

### ❌ Projeto não abre

* Verifique a versão do Unity

### ❌ Multiplayer não funciona

* Confirme se a App ID do Photon está configurada

### ❌ Erros de pacote

* Vá em:

```
Window → Package Manager
```

* Reinstale os pacotes necessários

---

## 👨‍💻 Equipe

* Gabriel Sales Dorea – Product Owner, Programador, Game Designer
* Carlos Daniel Hackmam – Scrum Master
* Artur Salvador Moro – Sound Designer
* Nikolas Bendinelli Vison – Modelador 3D
* Guilherme Castanho Jochi – Diretor de Arte
* Jair Rodrigues de Paula Junior – Documentação Auxiliar

---

## 📄 Observações

Este projeto foi desenvolvido como parte do curso de Jogos Digitais da Universidade de Sorocaba (UNISO).

---
