require 'green_shoes'

Shoes.app title: 'Eject', width: 550, height: 200 do
  para 'Input device', margin: 10
  @connect_line = edit_line width: 400, margin: 10
  @connect_button = button 'Connect', margin: 10
  @get_list = button 'Get list', margin: 10
  @get_list.click do
    Thread.new do
      Shoes.app title: 'Device list', width: 600, height: 1000 do
        Thread.new do
          @edit_box = edit_box width: 600, height: 1000
          loop do
            @name = Array.new()
            information = `sudo iwlist scan`
            i = 0
            j = 0
            information.split("\n").each do |line|
              if line.include?'ESSID'
                @name[i] = line.split(':')[1].delete('"\\"','\\""')
                @name[i] += '&'
              end
              if line.include?'Address'
                if j == 0
                  j += 1
                  next
                end
                @name[i] += line.split(' ').last
                i += 1
              end
            end
            information = `nmcli device wifi list`
            j = 0
            information.split("\n").each do |line|
              if j == 0
                j += 1
                @edit_box.text = "#{line.delete('*')}\n"
                next
              end
              @name.each do |item|
                @name_1 = if line.split(' ')[0] == '*'
                            line.split(' ')[1]
                          else
                            line.split(' ')[0]
                          end
                if item.split('&')[0].include?@name_1
                  @edit_box.text += "#{line}\n  MAC: #{item.split('&')[1]}\n"
                  break
                end
              end
            end
            sleep 1
          end
        end
      end
    end
  end
  @connect_button.click do
    `nmcli dev wifi connect #{@connect_line.text}`
  end
end
