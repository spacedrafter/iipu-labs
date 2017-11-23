class Device
  attr_accessor :vendor_id, :device_id, :vendor_name, :device_name

  def initialize(vendor_id = nil, device_id = nil, vendor_name = nil, device_name = nil)
    @vendor_id = vendor_id
    @device_id = device_id
    @vendor_name = vendor_name
    @device_name = device_name
  end

  def output
    print "#{@vendor_name.chomp} -- #{@device_name} "

  end

end
