# -*- coding: cp1251 -*-
import random
import json
import csv

def save_game_data(data):
    with open('data.json', 'w') as file:
        json.dump(data, file)

def load_game_data():
    try:
        with open('data.json', 'r') as file:
            data = json.load(file)
    except FileNotFoundError:
        data = {}
    return data

def update_csv(data):
    with open('game_data.csv', 'w', newline='') as file:
        writer = csv.writer(file)
        writer.writerow(["Игрок", "роль"])
        for player, role in data.items():
            writer.writerow([player, role])

def start_game(n):
    mafia = random.choice(n)
    b = {m: "Мафия" if m == mafia else "Граждане" for m in n}
    data = load_game_data()
    data.update(b)
    save_game_data(data)
    update_csv(data)
    print("Начало игры!")
    print("Мафия:", mafia)
    print("Граждане:", [player for player in n if player != mafia])



    players = []
    b = ['Мафия', 'Мирный житель', 'Мирный житель','Мирный житель','Мирный житель','Мирный житель']
    random.shuffle(b)
    
    # Создаем игроков и случайным образом распределяем роли
    for i in range(len(b)):
        player = {'Роль': b[i], 'Жив': True}
        players.append(player)
    
    # Цикл игры
    while True:
        for i in range(len(players)):
            print(f"Игрок {i+1}")
        
        # Запрос ввода игрока 
        vote = int(input("Кого хотите проверить? (Введите номер игрока): "))
        
        # Проверка валидности ввода
        if vote < 1 or vote > len(players):
            print("Неверный номер игрока, попробуйте еще раз.")
            continue
        
        # Проверка роли выбранного игрока
        if players[vote-1]['Роль'] == 'Мафия':
            print("Игрок номер", vote, "принадлежит к мафии!")
            break
        else:
            print("Игрок номер", vote, "не принадлежит к мафии!")
        
        # Проверка на победу одной из сторон
        mafia_count = 0
        citizens_count = 0
        for player in players:
            if player['Жив']:
                if player['Роль'] == 'Мафия':
                    mafia_count += 1
                else:
                    citizens_count += 1
        if mafia_count == 0:
            print("Мирные жители победили!")
            break
        elif mafia_count >= citizens_count:
            print("Мафия победила!")
            break

def delete_save_data():
    with open('data.json', 'w') as file:
        file.write("")
    with open('game_data.csv', 'w', newline='') as file:
        file.write("")

def main_menu():
    while True:
        print("1. Начало игры")
        print("2. Удалить сохранение")
        print("3. Выйти")
        choice = input("Введите свой выбор: ")
        if choice == "1":
            players = input("Введите имена игроков (через запятую): ").split(",")
            start_game(players)
        elif choice == "2":
            delete_save_data()
            print("Save data deleted.")
        elif choice == "3":
            break
        else:
            print("Неверный выбор. Пожалуйста, попробуйте снова.")

if __name__ == "__main__":
    main_menu()

