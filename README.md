# ⚔️ EDWIND — Huyền Thoại Edwind

> **Thể loại:** Phiêu lưu hành động 2D màn hình ngang bán tuyến tính (*2D Semi-Linear Side-Scrolling Adventure / Metroidvania*)  
> **Cảm hứng thiết kế:** *Ori and the Blind Forest*, *Ori and the Will of the Wisps*  
> **Engine:** Unity (Universal Render Pipeline 2D)

---

## 📖 Cốt truyện & Bối cảnh (Synopsis)

**Edwind** xoay quanh cuộc hành trình khám phá những bí ẩn bị lãng quên trong truyền thuyết về **Huyền Thoại Edwind**.

Trò chơi mở đầu tại một căn nhà gỗ tĩnh lặng, nơi ngọn lửa trong lò sưởi đá vẫn âm ỉ cháy qua năm tháng. Khi bước lại gần lò sưởi, ngồi xuống chiếc ghế tựa và lật mở cuốn sách cổ khắc dòng chữ **`EX LIBRIS — EDWIND`**, người chơi sẽ từng bước bước vào những chương biên niên sử đầu tiên, mở ra một thế giới màn hình ngang rộng lớn, bán tuyến tính với nhiều vùng đất kết nối liền mạch, những tàn tích cổ xưa và các năng lực di chuyển chờ được đánh thức.

---

## 🌟 Định hướng Gameplay & Trụ cột Thiết kế (Core Pillars)

1. **Thế giới Màn Hình Ngang Bán Tuyến Tính (Semi-Linear World Design):**
   - Bản đồ được thiết kế theo trục ngang có chiều sâu, kết nối nhiều khu vực với nhau tương tự dòng game *Ori*.
   - Người chơi đi theo mạch truyện chính nhưng luôn có các nhánh rẽ, khu vực ẩn và những lối đi đòi hỏi mở khóa kỹ năng mới để quay lại khám phá.
2. **Không gian 2D có Chiều sâu & Ánh sáng Cảm xúc (Atmospheric 2.5D & Dynamic Lighting):**
   - Kết hợp kỹ thuật tách lớp tiền cảnh / trung cảnh / hậu cảnh (`Layer Sandwich & Sorting Order`) để nhân vật tương tác tự nhiên với môi trường xung quanh.
   - Sử dụng hệ thống chiếu sáng động **URP 2D (`Global Light 2D` & `Point Light 2D`)** tạo bầu không khí ấm áp hoặc huyền bí theo từng khu vực.
3. **Giao diện Hòa nhập Thế giới (Diegetic Storytelling UI):**
   - Các điểm nghỉ chân (như chiếc ghế bên lò sưởi) đóng vai trò là nơi nhân vật dừng lại, mở cuốn sách biên niên sử **Edwind** để tiếp tục hành trình (*Chơi Tiếp*) hoặc viết lại chương mới (*Chơi Mới*).

---

## ⚙️ Các hệ thống đã triển khai (Current Features)

- **Cảnh mở đầu (Prologue Cabin Scene):**
  - Không gian nội thất căn nhà gỗ với tường gỗ, sàn gỗ, lò sưởi đá khắc khối và ghế Sofa tách lớp chiều sâu (`Sofa_Back` & `Sofa_FrontArm`).
- **Hệ thống Hoạt ảnh 2D (2D Animation & Animator State Machine):**
  - **Nhân vật chính:** Máy trạng thái `Player.controller` chuyển đổi tức thời giữa các trạng thái `Player_Idle` (đứng thở nhẹ), `Player_Walk` (chu kỳ bước chân 4 nhịp) và `Player_Sit` (ngồi nghỉ bên lò sưởi).
  - **Môi trường sống động:** Hoạt ảnh lửa cháy 4 frame (`Fireplace_Flame.anim`) kết hợp script `FireplaceAnimator2D` dùng nhiễu `PerlinNoise` làm ánh sáng `Point Light 2D` bập bùng theo ngọn lửa.
- **Hệ thống Camera Điện ảnh (`CameraFollow2D`):**
  - Bám theo chuyển động ngang của nhân vật mượt mà bằng `Vector3.SmoothDamp` trong `LateUpdate()`, tự động khóa biên theo kích thước từng căn phòng/khu vực (`Room Bounds Clamping`).
- **Hệ thống Sách Biên Niên Sử (`BookMenuUI`):**
  - Giao diện uGUI cuốn sách dày mở bìa với hiệu ứng chuyển cảnh `CanvasGroup` (`Fade & Scale Pop-in`), tương thích hoàn toàn với `Unity New Input System`.

---

## 🗺️ Lộ trình phát triển tiếp theo (Development Roadmap)

- [x] **Giai đoạn 1 — Prologue Scene & Core Interaction:** Dựng cảnh căn nhà gỗ, hệ thống tách lớp chiều sâu, ngồi ghế mở sách Biên Niên Sử.
- [x] **Giai đoạn 2 — 2D Animation & Dynamic Lighting:** Hoàn thiện Walk Cycle, Idle, lửa cháy và ánh sáng lò sưởi động.
- [ ] **Giai đoạn 3 — Vật lý, Nhảy & Cơ chế Platformer (Ori-like Mobility):** Tích hợp `Rigidbody2D`, `Collider2D`, nhảy mượt (Coyote Time, Jump Buffering), leo tường / lướt (Dash).
- [ ] **Giai đoạn 4 — Mở rộng Thế giới Bán Tuyến Tính:** Kết nối cửa ra vào căn nhà gỗ với khu rừng bên ngoài, hệ thống chuyển vùng (Room/Zone Transitions) và Parallax Background nhiều lớp.
- [ ] **Giai đoạn 5 — Âm thanh & Hiệu ứng Thời tiết:** Nhạc nền cảm xúc, tiếng củi cháy, tiếng bước chân và hạt bụi sáng (Particle System).

---

## 📝 Nhật ký Cập nhật (Devlog & Updates)

### 📅 Day 1 — Mở rộng Di chuyển Trục Y & Bài toán Giới hạn Không gian Phòng

🎬 **Video Demo Day 1:** [Xem video `day1.mp4`](video/day1.mp4)

<video src="video/day1.mp4" controls width="100%"></video>

#### 1. Bổ sung khả năng di chuyển Lên / Xuống (Trục Y)
- Mở rộng `PlayerController2D.cs` để nhận thêm tín hiệu phím **`W` / `S`** (hoặc phím mũi tên **`↑` / `↓`**), cho phép nhân vật di chuyển 4 hướng trên toàn bộ bề mặt sàn gỗ thay vì chỉ đi trên một đường thẳng ngang duy nhất.
- Giữ nguyên cơ chế lật hướng mặt (`flipX`) theo trục ngang `X` và tự động kích hoạt trạng thái `IsWalking` trên `Animator` khi nhân vật di chuyển theo bất kỳ trục nào (`X` hoặc `Y`).

#### 2. Giới hạn vùng di chuyển: Dùng chỉ số `Position` (`Mathf.Clamp`) hay gắn `Rigidbody2D` / `Collider2D` cho tường?
Trong quá trình xây dựng căn phòng có chiều sâu 2.5D, có hai phương pháp để ngăn nhân vật đi xuyên lên mảng tường phía sau hoặc đi ra ngoài phạm vi sàn:

- **Phương pháp 1 — Giới hạn trực tiếp bằng chỉ số tọa độ (`Position Clamping` với `Mathf.Clamp`):** *(Đang áp dụng trong Day 1)*
  - Khóa trực tiếp tọa độ `transform.position` trong khung hình chữ nhật của mặt sàn (`minX = -7.5`, `maxX = 7.5`, `minY = -2.6`, `maxY = -1.0`).
  - **Ưu điểm:** Thiết lập nhanh, cực kỳ nhẹ (không tốn chi phí tính toán vật lý), không bị lỗi kẹt góc collider, rất hiệu quả cho các căn phòng có mặt sàn hình chữ nhật phẳng.
  - **Hạn chế:** Chỉ giới hạn được khung biên hình chữ nhật ngoài cùng; chưa tự chặn được các vật cản nằm giữa sàn (như bệ lò sưởi hoặc thân ghế Sofa).

- **Phương pháp 2 — Sử dụng Hệ thống Vật lý (`Rigidbody2D` + `Collider2D` cho tường và đồ vật):** *(Hướng nâng cấp khi mở rộng màn chơi)*
  - Gắn `Rigidbody2D` (`Gravity Scale = 0` trong phòng 2.5D hoặc có trọng lực ở các màn platformer, bật `Freeze Rotation Z`) và một `BoxCollider2D` nhỏ đặt ở **vùng bàn chân** của nhân vật.
  - Gắn `BoxCollider2D` (hoặc `TilemapCollider2D` / `CompositeCollider2D`) cho ranh giới chân tường, chân lò sưởi và khung đế của ghế Sofa.
  - **Ưu điểm:** Nhân vật có thể đi vòng qua mặt trước/mặt bên của lò sưởi và ghế sofa mà bị chặn lại tự nhiên khi đụng trúng chân vật thể, đồng thời sẵn sàng cho các cơ chế va chạm, bậc thang và địa hình phức tạp của thế giới bán tuyến tính sau này.
