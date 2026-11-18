# 🏡 Edwind — Dự Án Học Làm Game 2D Unity (Top-Down 3/4 RPG)

**Edwind** là dự án thực hành phát triển game 2D bằng **Unity (Universal Render Pipeline 2D)**. Cảnh mở đầu tái hiện không gian bên trong một **căn nhà gỗ ấm cúng** với lò sưởi đá bập bùng lửa ấm, chiếc ghế Sofa êm ái đặt cạnh lò sưởi, nhân vật có thể đi lại, ngồi vào ghế sưởi ấm và mở cuốn **Nhật Ký Căn Nhà Gỗ** (Diegetic Book Menu UI).

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

## 🚀 Cách mở và chạy dự án

1. Mở **Unity Hub**, chọn **Add -> Add project from disk** và trỏ tới thư mục `d:\Unity\project\Edwind`.
2. Trong cửa sổ **Project** của Unity Editor, mở scene tại đường dẫn `Assets/Scenes/SampleScene.unity`.
3. Nhấn nút **Play (▶)** ở thanh công cụ phía trên để bắt đầu trải nghiệm.
