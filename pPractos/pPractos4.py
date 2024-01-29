# -*- coding: utf-8 -*-
class Users:
    def __init__(self):
        self.users = []

    def register_user(self, username, password, role):
        self.users.append({
            'username': username,
            'password': password,
            'role': role
        })

    def get_user_by_username(self, username):
        for user in self.users:
            if user['username'] == username:
                return user
        return None

    def update_user_data(self, username, new_data):
        user = self.get_user_by_username(username)
        if user:
            user.update(new_data)

    def remove_user(self, username):
        user = self.get_user_by_username(username)
        if user:
            self.users.remove(user)


class outfit:
    def __init__(self):
        self.outfit = []

    def add_outfit (self, brand, model, year, color, price, features):
        self.outfit.append({
            'brand': brand,
            'model': model,
            'year': year,
            'color': color,
            'price': price,
            'features': features
        })

    def remove_outfit(self, brand, model):
        for outfit in self.outfit:
            if outfit['brand'] == brand and outfit['model'] == model:
                self.outfit.remove(outfit)
                break

    def get_all_coutfit(self):
        return self.outfit

    def filter_outfit(self, keyword):
        filtered_outfit = []
        for outfit in self.outfit:
            if keyword.lower() in outfit['model'].lower() or keyword.lower() in outfit['brand'].lower():
                filtered_outfit.append(outfit)
        return filtered_outfit


class Orders:
    def __init__(self):
        self.orders = []

    def add_order(self, username, outfit):
        self.orders.append({
            'username': username,
            'outfit': outfit
        })

    def remove_order(self, username, outfit):
        for order in self.orders:
            if order['username'] == username and order['outfit'] == outfit:
                self.orders.remove(order)
                break

    def get_user_orders(self, username):
        user_orders = []
        for order in self.orders:
            if order['username'] == username:
                user_orders.append(order)
        return user_orders


class Database:
    def __init__(self):
        self.users = Users()
        self.outfit = outfit()
        self.orders = Orders()

    def register_user(self, username, password, role):
        self.users.register_user(username, password, role)

    def login_user(self, username, password):
        user = self.users.get_user_by_username(username)
        if user and user['password'] == password:
            return user
        return None

    def add_outfit(self, brand, model, year, color, price, features):
        self.outfit.add_outfit(brand, model, year, color, price, features)

    def remove_outfit(self, brand, model):
        self.outfit.remove_outfit(brand, model)

    def get_all_outfit(self):
        return self.outfit.get_all_outfit()

    def filter_outfit(self, keyword):
        return self.outfit.filter_outfit(keyword)

    def add_order(self, username, outfit):
        self.orders.add_order(username, outfit)

    def remove_order(self, username, outfit):
        self.orders.remove_order(username, outfit)

    def get_user_orders(self, username):
        return self.orders.get_user_orders(username)


def main():
    database = Database()

    while True:
        print("Добро пожаловать!")
        print("1. Зарегистрироваться")
        print("2. Вход в аккаунт")
        print("3. Выход")

        choice = input("Выберите опцию: ")

        if choice == '1':
            username = input("Введите имя: ")
            password = input("Введите пароль: ")
            role = input("Выберите роль (1 - Клиент, 2 - Сотрудник, 3 - Администратор): ")

            if role == '1':
                role = 'Клиент'
            elif role == '2':
                role = 'Сотрудник'
            elif role == '3':
                role = 'Администратор'

            database.register_user(username, password, role)
            print("✅ Регистрация прошла успешно!")

        elif choice == '2':
            username = input("Введите имя: ")
            password = input("Введите пароль: ")

            user = database.login_user(username, password)

            if user:
                print(f"🎉 Добро пожаловать, {user['username']} ({user['role']})!")

                if user['role'] == 'Клиент':
                    while True:
                        print("\n Выберите действие:")
                        print("1. Просмотреть всю одежду")
                        print("2. Фильтрация одежды")
                        print("3. Добавить одежду в заказ")
                        print("4. Снять одежду с заказа")
                        print("5. Обновлять пользовательские данные")
                        print("6. Выход")

                        customer_choice = input("Выберите действие: ")

                        if customer_choice == '1':
                            outfit = database.get_all_outfit()
                            if outfit:
                                print("\nДоступные автомобили:")
                                for outfit in outfit:
                                    print(f"  - {outfit['brand']} {outfit ['model']}")
                            else:
                                print("\nДоступной одежды нет.")

                        elif customer_choice == '2':
                            keyword = input("Введите ключевое слово для фильтрации одежды: ")
                            filtered_outfit = database.filter_outfit(keyword)
                            if filtered_outfit:
                                print(f"\nОдежда, соответствующая фильтру '{keyword}':")
                                for outfit in filtered_outfit:
                                    print(f"  - {outfit['brand']} {outfit['model']}")
                            else:
                                print("\nНет одежды, подходящей под фильтр.")

                        elif customer_choice == '3':
                            brand = input("Введите бренд одежды: ")
                            model = input("Введите модель одежды: ")
                            database.add_order(username, f"{brand} {model}")
                            print("✅ Одежда добавлена к заказу.")

                        elif customer_choice == '4':
                            brand = input("Введите бренд одежды: ")
                            model = input("Введите модель одежды: ")
                            database.remove_order(username, f"{brand} {model}")
                            print("✅ Одежда снята с заказа.")

                        elif customer_choice == '5':
                            new_username = input("Введите новое имя пользователя: ")
                            database.update_user_data(username, {'username': new_username})
                            print("✅ Пользовательские данные успешно обновлены.")

                        elif customer_choice == '6':
                            break

                        else:
                            print("❌ Неверный выбор. Пожалуйста, попробуйте снова.")

                elif user['role'] == 'Сотрудник':
                            while True:
                                print("\n Выберите действие:")
                                print("1. Просмотреть всю одежду")
                                print("2. Фильтрация одежды")
                                print("3. Добавить одежду")
                                print("4. Снять одежду")
                                print("5. Обновлять пользовательские данные")
                                print("6. Выход")

                                employee_choice = input("Выберите действие: ")

                                if employee_choice == '1':
                                    outfit = database.get_all_outfit()
                                    if outfit:
                                        print("\nДоступные автомобили:")
                                        for outfit in outfit:
                                            print(f"  - {outfit['brand']} {outfit['model']}")
                                    else:
                                        print("\nСвободной одежды нет.")

                                elif employee_choice == '2':
                                    keyword = input("Введите ключевое слово для фильтрации одежды: ")
                                    filtered_outfit = database.filter_outfit(keyword)
                                    if filtered_outfit:
                                        print(f"\nОдежда, соответствующая фильтру '{keyword}':")
                                        for outfit in filtered_outfit:
                                            print(f"  - {outfit['brand']} {outfit['model']}")
                                    else:
                                        print("\nНет одежды, подходящей под фильтр.")

                                elif employee_choice == '3':
                                    brand = input("Введите бренд одежды: ")
                                    model = input("Введите модель одежды: ")
                                    year = input("Введите год выпуска одежды: ")
                                    color = input("Введите цвет одежды: ")
                                    price = input("Введите цену одежды: ")
                                    features = input("Введите характеристики одежды (через запятую): ")

                                    database.add_outfit(brand, model, year, color, price, features)
                                    print("✅ Одежда успешно добавлен.")

                                elif employee_choice == '4':
                                    brand = input("Введите бренд одежды: ")
                                    model = input("Введите модель одежды: ")

                                    database.remove_car(brand, model)
                                    print("✅ Одежда успешно удалена.")

                                elif employee_choice == '5':
                                    new_username = input("Введите новое имя пользователя: ")
                                    database.update_user_data(username, {'username': new_username})
                                    print("✅ Пользовательские данные успешно обновлены.")

                                elif employee_choice == '6':
                                    break

                                else:
                                    print("❌ Неверный выбор. Пожалуйста, попробуйте снова.")

                elif user['role'] == 'Администратор':
                        while True:
                                print("\n Выберите действие:")
                                print("1. Просмотреть всю одежду")
                                print("2. Фильтрация одежды")
                                print("3. Добавить одежду")
                                print("4. Снять одежду")
                                print("5. Обновлять пользовательские данные")
                                print("6. Удалить пользователя")
                                print("7. Выход")

                                admin_choice = input("Выберите действие: ")

                                if admin_choice == '1':
                                    outfit = database.get_all_outfit()
                                    if outfit:
                                        print("\nДоступная одежда:")
                                        for outfit in outfit:
                                            print(f"  - {outfit['brand']} {outfit['model']}")
                                    else:
                                        print("\nДоступной одежды нет.")

                                elif admin_choice == '2':
                                    keyword = input("Введите ключевое слово для фильтрации одежды: ")
                                    filtered_outfit = database.filter_outfit(keyword)
                                    if filtered_outfit:
                                        print(f"\nОдежда, соответствующая фильтру '{keyword}':")
                                        for outfit in filtered_outfit:
                                            print(f"  - {outfit['brand']} {outfit['model']}")
                                    else:
                                        print("\nНет одежды, подходящей под фильтр.")

                                elif admin_choice == '3':
                                    brand = input("Введите бренд одежды: ")
                                    model = input("Введите модель одежды: ")
                                    year = input("Введите год выпуска одежды: ")
                                    color = input("Введите цвет одежды: ")
                                    price = input("Введите цену одежды: ")
                                    features = input("Введите характеристики одежды (через запятую): ")

                                    database.add_outfit(brand, model, year, color, price, features)
                                    print("✅ Одежды успешно добавлена.")

                                elif admin_choice == '4':
                                    brand = input("Введите бренд одежды: ")
                                    model = input("Введите модель одежды: ")

                                    database.remove_car(brand, model)
                                    print("✅ Одежда успешно удалена.")
                                    
                                elif admin_choice == '5':
                                    new_username = input("Введите новое имя пользователя: ")
                                    database.update_user_data(username, {'username': new_username})
                                    print("✅ Пользовательские данные успешно обновлены.")

                                elif admin_choice == '6':
                                    remove_username = input("Введите имя пользователя для удаления: ")
                                    database.remove_user(remove_username)
                                    print("✅ Пользователь успешно удален.")

                                elif admin_choice == '7':
                                    break

                                else:
                                    print("❌ Неверный выбор. Пожалуйста, попробуйте снова.")

            else:
                print("❌ Неверное имя пользователя или пароль. Пожалуйста, попробуйте снова.")

        elif choice == '3':
            print("👋 До свидания!")
            break

        else:
            print("❌ Invalid choice. Please try again.")


if __name__ == "__main__":
    main()
