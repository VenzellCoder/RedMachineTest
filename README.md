# RedMachineTest
 
Добрый день!

Спасибо за интересное тех. задание.
На выполнение ушло 4 вечера (так как днём - основная работа).

Примечания:
- Пространственное разделение позволило увеличить количество юнитов в симуляции до 3000. На телефоне Pocophone F1 40-70 FPS, без пространственного разделения FPS был 1-5. Так как пропорции игрового поля, количество юнитов и их размер могут быть любыми, необходимо руками указывать количество ячеек (константы в SpatialPartitionGrid.cs). Для 3000 юнитов подошло поле 50х50 ячеек
- Реализовал пул объектов для юнитов
- Реализовал загрузку GameConfig без привязки к конкретной реализации (из-за особенностей разных платформ)
- Процесс боя представлен семейством классов в фасаде Battle.cs
- В задании речь шла о двух командах. Но большой разницы в разработке логики для двух или N команд нет, поэтому в GameConfig.json можно указать любое количество команд.

Буду благодарен за обратную связь.
Спасибо.
