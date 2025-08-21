# 🎮 Game Talk with AI-chan  

> Một trải nghiệm **AR + AI** kết hợp giữa **LLaMA**, **TTS**, và **STT** trên nền tảng **Unity**.  
> Bạn có thể trò chuyện trực tiếp với nhân vật ảo trong không gian thực tế tăng cường.  

---

## ✨ Giới thiệu  

**Talk with AI-chan** là một trò chơi AR nơi người chơi có thể:  
- 🗣 **Nói chuyện tự nhiên** với nhân vật AI bằng giọng nói.  
- 🤖 AI sử dụng **LLaMA** để xử lý ngôn ngữ và đưa ra câu trả lời thông minh.  
- 🔊 Trả lời của AI được phát ra bằng giọng nói nhờ **Text-to-Speech (TTS)**.  
- 👂 Người chơi nói bằng micro, hệ thống **Speech-to-Text (STT)** sẽ nhận diện lời nói và gửi cho AI.  
- 🌎 Nhân vật **AI-chan** được hiển thị dưới dạng 3D trong không gian AR, tạo cảm giác như một người bạn đồng hành ngay bên cạnh.  
- 🧑‍🎤 **Nhân vật 3D (VRoid model)** sẽ thay đổi **sắc mặt, biểu cảm, hành động** tùy theo lời nói và yêu cầu của người chơi.  

---

## 🛠 Công nghệ sử dụng  

- **Unity + AR Foundation** → xây dựng AR trên cả Android và iOS.  
- **LLaMA (Large Language Model)** → xử lý hội thoại, phản hồi tự nhiên và sáng tạo.  
- **Android Plugin (Java/Kotlin)**:  
  - **STT Plugin** → sử dụng `SpeechRecognizer` của Android để nhận giọng nói.  
  - **TTS Plugin** → sử dụng `TextToSpeech` của Android để phát âm giọng AI.  
- **Unity ↔ Android Bridge** → kết nối C# và plugin Android qua `AndroidJavaClass`, `AndroidJavaObject`, `UnitySendMessage`.  
- **VRoid 3D Model** → nhân vật AI-chan, hỗ trợ thay đổi **biểu cảm** và **animation** thông qua Unity Animator.  

---

## 🎮 Gameplay Flow  

1. 👩 Người chơi nói chuyện với AI-chan (VD: “Xin chào AI-chan, hôm nay thế nào?”).  
2. 🎤 **STT Plugin** lắng nghe và chuyển giọng nói thành text.  
3. 📡 Text được gửi tới **LLaMA model** (chạy local hoặc server).  
4. 🤖 AI-chan sinh câu trả lời → trả về Unity.  
5. 🔊 **TTS Plugin** đọc to câu trả lời bằng giọng tự nhiên.  
6. 🧑‍🎤 AI-chan trong Unity (VRoid 3D model) hiển thị:  
   - Thay đổi **biểu cảm gương mặt** (vui, buồn, ngạc nhiên, giận dữ, v.v.)  
   - Chạy **animation hành động** (vẫy tay, gật đầu, suy nghĩ, nhảy cười, v.v.)  
   - Phản ứng theo **ngữ cảnh và yêu cầu** của người chơi.  
7. 🕶 Người chơi thấy AI-chan trong AR **trò chuyện và biểu lộ cảm xúc sống động**.  

---
