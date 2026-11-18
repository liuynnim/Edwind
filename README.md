# 🏡 Edwind — Dự Án Học Làm Game 2D Unity (Top-Down 3/4 RPG)

**Edwind** là dự án thực hành phát triển game 2D bằng **Unity (Universal Render Pipeline 2D)**. Cảnh mở đầu tái hiện không gian bên trong một **căn nhà gỗ ấm cúng** với lò sưởi đá bập bùng lửa ấm, chiếc ghế Sofa êm ái đặt cạnh lò sưởi, nhân vật có thể đi lại, ngồi vào ghế sưởi ấm và mở cuốn **Nhật Ký Căn Nhà Gỗ** (Diegetic Book Menu UI).

---

## ✨ Tính năng đã hoàn thiện

1. **Đồ họa Pixel Art Top-Down 3/4 RPG (`64 PPU`):**
   - Tường gỗ và sàn gỗ vát cạnh (`Bevel` + `1px Rim Highlight`), lò sưởi đá khắc khối, ghế sofa và nhân vật Chibi RPG kèm bóng đổ mềm bán trong suốt (`Soft Drop Shadow`).
   - Hệ thống ánh sáng **URP 2D** gồm `Global Light 2D` và `Point Light 2D` tỏa ánh sáng cam ấm áp từ lò sưởi.
2. **Hiệu ứng chiều sâu 2.5D khi ngồi ghế (`Sorting Order Layering`):**
   - Chiếc ghế Sofa được tách thành 2 lớp Sprite (`Sofa_Back` ở `Order = 2` và `Sofa_FrontArm` ở `Order = 4`), giúp nhân vật (`Order = 3`) ngồi lọt vào trong lòng ghế tự nhiên.
3. **Điều khiển nhân vật & Tương tác (`New Input System`):**
   - Đi bộ ngang mượt mà, tự động lật hướng nhìn (`flipX`) và kiểm tra khoảng cách tương tác với ghế Sofa (`SofaInteractable`).
4. **Camera bám theo nhân vật mượt mà (`CameraFollow2D`):**
   - Sử dụng `Vector3.SmoothDamp` trong `LateUpdate()` giúp triệt tiêu hiện tượng giật khung hình (*Jitter*).
   - Tích hợp khóa biên căn phòng (`Room Bounds Clamping`) để camera không bao giờ nhìn lọt ra khoảng đen bên ngoài tường.
5. **Giao diện Cuốn Sách Nhật Ký (`BookMenuUI` - Diegetic UI):**
   - Khi nhân vật ngồi xuống ghế, cuốn sách dày mở bìa xuất hiện với hiệu ứng mờ dần & phóng nhẹ (`Fade & Scale Pop-in`).
   - Nửa bên trái là mặt bìa trong bọc da mạ đồng (`EX LIBRIS - EDWIND`), nửa bên phải là mặt giấy ngà chứa các nút **CHƠI TIẾP**, **CHƠI MỚI**, **GẤP SÁCH**.

---

## 🎮 Hướng dẫn điều khiển (Controls)

| Phím / Thao tác | Chức năng |
| :--- | :--- |
| **`A` / `D`** hoặc **`←` / `→`** | Di chuyển nhân vật qua trái / phải trên sàn nhà |
| **`E`** hoặc **`Space`** | Ngồi vào ghế Sofa (khi đứng gần ghế) & mở Cuốn Sách Menu |
| **Chuột trái (`Left Click`)** | Chọn **`CHƠI TIẾP`**, **`CHƠI MỚI`** hoặc **`GẤP SÁCH`** trên trang giấy phải |
| **`ESC`** | Gấp sách nhanh và đứng dậy khỏi ghế Sofa |

---

## 📂 Cấu trúc thư mục dự án

```text
Edwind/
├── Assets/
│   ├── Scenes/
│   │   └── SampleScene.unity          # Cảnh căn phòng gỗ chính của game
│   ├── Scripts/
│   │   ├── PlayerController2D.cs      # Điều khiển di chuyển & ngồi ghế Sofa
│   │   ├── SofaInteractable.cs        # Quản lý vùng tương tác & điểm neo ngồi (SitAnchor)
│   │   ├── CameraFollow2D.cs          # Camera bám theo nhân vật mượt mà + khóa biên phòng
│   │   └── BookMenuUI.cs              # Giao diện uGUI Cuốn Sách Menu khi ngồi ghế
│   ├── Textures/
│   │   ├── wall wood texture 1.png    # Texture tường gỗ (64x64, Tiled)
│   │   ├── floor_wood_texture.png     # Texture sàn gỗ (64x64, Tiled)
│   │   ├── fireplace.png              # Sprite lò sưởi đá khắc khối (128x128)
│   │   ├── sofa_back.png              # Lớp sau của ghế Sofa (128x128, Order = 2)
│   │   ├── sofa_front_arm.png         # Lớp tay vịn trước của ghế Sofa (128x128, Order = 4)
│   │   ├── player_standing.png        # Nhân vật tư thế đứng (64x96, Order = 3)
│   │   ├── player_sitting.png         # Nhân vật tư thế ngồi (64x96, Order = 3)
│   │   └── book_menu.png              # Giao diện cuốn sách mở bìa (560x360)
│   └── Settings/                      # Cấu hình URP 2D Renderer & Lit2D Scene Template
├── HUONG_DAN_LO_TRINH_HOC_2D.md       # Cẩm nang 10 chương lộ trình học làm game 2D từ A-Z
├── HUONG_DAN_TAO_BOOK_MENU_UI.md      # Tài liệu chuyên sâu về cách dựng UI Cuốn Sách
└── README.md                          # Thông tin tổng quan dự án
```

---

## 📚 Tài liệu học tập đi kèm trong dự án

Dự án có sẵn 2 tài liệu hướng dẫn chi tiết bằng tiếng Việt để bạn vừa thực hành vừa tra cứu:
1. 📘 **[HUONG_DAN_LO_TRINH_HOC_2D.md](file:///d:/Unity/project/Edwind/HUONG_DAN_LO_TRINH_HOC_2D.md):**
   - Giải thích tư duy phối cảnh 2.5D Oblique và Top-Down 3/4 RPG.
   - Kỹ thuật tách lớp Sprite (`Layer Sandwich`), chuẩn hóa `64 PPU`, `Point Filter`, `Orthographic Camera`.
   - Giải thích code C# di chuyển, tương tác, `CameraFollow2D`, cùng hướng dẫn nâng cấp lên di chuyển 4 hướng (`W/A/S/D`) và `Y-Sorting` tự động.
2. 📙 **[HUONG_DAN_TAO_BOOK_MENU_UI.md](file:///d:/Unity/project/Edwind/HUONG_DAN_TAO_BOOK_MENU_UI.md):**
   - Phân tích cấu trúc thị giác cuốn sách mở bìa, kiến trúc `Canvas`, `CanvasScaler`, `CanvasGroup`, `InputSystemUIInputModule` và hiệu ứng chuyển động UI.

---

## 🚀 Cách mở và chạy dự án

1. Mở **Unity Hub**, chọn **Add -> Add project from disk** và trỏ tới thư mục `d:\Unity\project\Edwind`.
2. Trong cửa sổ **Project** của Unity Editor, mở scene tại đường dẫn `Assets/Scenes/SampleScene.unity`.
3. Nhấn nút **Play (▶)** ở thanh công cụ phía trên để bắt đầu trải nghiệm.
