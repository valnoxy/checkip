//
//  ContentView.swift
//  CheckIP
//
//  Created by Jonas Günner on 28.03.23.
//

import SwiftUI

struct Option: Hashable {
    let title: String
    let imageName: String
}

struct ContentView: View {
    @State var currentOption = 0
    
    let options: [Option] = [
        .init(title: "Fetch", imageName: "magnifyingglass"),
        .init(title: "My IP", imageName: "location.fill"),
        .init(title: "About", imageName: "info.circle"),
    ]
    
    var body: some View {
        NavigationView () {
            ListView(
                options: options,
                currentSelection: $currentOption
            )
            
            switch currentOption {
            case 0:
                FetchView()
            case 1:
                MyIPView()
            case 2:
                AboutView()
            default:
                FetchView()
            }
        }
        .frame(minWidth: 600, minHeight: 400)
    }
}

struct ListView: View {
    let options: [Option]
    @Binding var currentSelection: Int
    
    var body: some View {
        VStack {
            let current = options[currentSelection]
            ForEach(options, id: \.self) { option in
                HStack {
                    Image(systemName: option.imageName)
                        .resizable()
                        .aspectRatio(contentMode: .fit)
                        .frame(width: 20)
                    Text(option.title)
                        .foregroundColor(current == option ?
                                         Color(.red) : Color(.white))
                        
                    Spacer()
                }
                .padding()
                .onTapGesture {
                    switch (option.title) {
                    case "Fetch":
                        self.currentSelection = 0
                    case "My IP":
                        self.currentSelection = 1
                    case "About":
                        self.currentSelection = 2
                    default:
                        self.currentSelection = 0
                    }
                }
            }
            Spacer()
            Text("Debug Build\nMy pp is burning")
                .foregroundColor(Color.red)
                .multilineTextAlignment(TextAlignment.center)
            Spacer()
        }
    }
}

struct FetchView: View {
    var body: some View {
        Text("Main View yay")
    }
}

struct MyIPView: View {
    var body: some View {
        Text("Main View yay")
    }
}

struct AboutView: View {
    var body: some View {
        Text("Main View yay")
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
